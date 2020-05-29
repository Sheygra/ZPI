using ZPI_Projekt_Anonimizator.entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Security.Permissions;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Diagnostics.Tracing;
using ZPI_Projekt_Anonimizator.Generators;

//Node of a Tree
class Node
{
	List<Node> nodes = new List<Node>(0);
	char sign;
	Node parent;
	int counter;
	bool end;
	int category;
	Stack<string> stack = null;

	public Node()
	{
		this.parent = null;
		this.category = -1;
		this.counter = 0;
		this.end = false;
	}

	public Node(char sign)
	{
		this.sign = sign;
		this.counter = 1;
	}

	public Node(char sign, Node parent)
	{
		this.sign = sign;
		this.counter = 1;
		this.parent = parent;
	}

	public Node(char sign, Node parent, int category)
	{
		this.sign = sign;
		this.counter = 1;
		this.parent = parent;
		this.category = category;
	}

	public void add(Node node)
	{
		nodes.Add(node);
	}

	public void add(char sign)
	{
		nodes.Add(new Node(sign));
	}

	public void add(char sign, Node parent)
	{
		nodes.Add(new Node(sign, parent));
	}

	public void add(char sign, Node parent, int category)
	{
		nodes.Add(new Node(sign, parent, category));
	}

	public List<Node> getNodes()
	{
		return nodes;
	}

	public char getSign()
	{
		return sign;
	}

	public void setSign(char sign)
	{
		this.sign = sign;
	}

	public Node getParent()
	{
		return parent;
	}

	public void setParent(Node parent)
	{
		this.parent = parent;
	}

	public Node getNode(char sign)
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			if (nodes[i].getSign() == sign)
			{
				return nodes[i];
			}
		}
		return null;
	}

	public bool isEnd()
	{
		return end;
	}

	public void setEnd(bool end)
	{
		this.end = end;
	}

	public int getCounter()
	{
		return counter;
	}

	public void setCounter(int counter)
	{
		this.counter = counter;
	}

	public void CounterPlusOne()
	{
		this.counter++;
	}

	public void CounterMinusOne()
	{
		this.counter--;
	}

	public void setCategory(int category)
	{
		this.category = category;
	}

	public int getCategory()
	{
		return this.category;
	}

	public void addToStack(string info)
	{
		if (stack == null)
		{
			stack = new Stack<string>();
		}
		stack.Push(info);
	}

	public string getFromStack()
	{
		return stack.Pop();
	}

	public int stackNull()
	{
		return stack.Count;
	}
}

//Tree for KAnonymization
public class KAnonymization
{
	private Node root;
	private int slowa = 0;
	private int wezly = 0;

	public KAnonymization()
	{
		root = new Node();
	}

	public int getSlowa()
	{
		return slowa;
	}

	public int getwezly()
	{
		return wezly;
	}


	//Addind data to the tree for DataTable
	public void add(DataTable patients, int K)
	{
		string data = "";
		int category = 0;
		Node node = root;
		bool check = false;
		for (int i = 0; i < patients.Rows.Count; i++)
		{
			Console.WriteLine(i);
			for (int j = 1; j < patients.Columns.Count - 1; j++)
			{
				data += ";" + patients.Rows[i].ItemArray[j];
			}
			category = 0;
			root.CounterPlusOne();
			node = root;
			check = false;
			while (data.Length > patients.Columns.Count - 2)
			{
				for (int k = 0; k < data.Length - 1; k++)
				{
					if (data[k] == ';')
					{
						if ((!data[k + 1].Equals(';')))
						{
							for (int j = 0; j < node.getNodes().Count; j++)
							{
								if (node.getNodes()[j].getSign() == data[k + 1] && node.getNodes()[j].getCategory() == category)
								{
									j = node.getNodes().Count;
									node = node.getNode(data[k + 1]);
									node.CounterPlusOne();
									check = true;
								}
							}

							if (check == false)
							{
								node.add(data[k + 1], node, category);
								node.getNode(data[k + 1]).setParent(node);
								node = node.getNode(data[k + 1]);
								wezly++;
							}
							check = false;
							data = data.Remove(k + 1, 1);
						}
						category++;
					}
				}
				category = 0;
			}
			node.setEnd(true);
			Console.WriteLine(patients.Rows[i].ItemArray[0] + ";" + patients.Rows[i].ItemArray[4]);
			node.addToStack(patients.Rows[i].ItemArray[0] + ";" + patients.Rows[i].ItemArray[4]);
			data = "";
		}
		k_anonymization(K);
	}

	//Anonymization of the tree
	private void k_anonymization(int k)
	{
		Node node = root;

		if (node.isEnd() == true)
		{
			return;
		}

		Stack<Node> nodeStack = new Stack<Node>();
		nodeStack.Push(node);

		while (nodeStack.Count > 0)
		{

			node = nodeStack.Peek();
			nodeStack.Pop();

			for (int i = 0; i < node.getNodes().Count; i++)
			{
				if (node.getNodes()[i].getCounter() < k)
				{
					node.getNodes()[i].setSign('*');
				}
				else nodeStack.Push(node.getNodes()[i]);
			}
		}
	}

	//Getting data back into DataTable
	public DataTable normalize()
	{
		DocumentGenerator DOCXGen = new DOCXGenerator();
		DocumentGenerator JPGGen = new JPGGenerator();
		DocumentGenerator DICOMGen = new DICOMGenerator();
		DataTable patients = new DataTable();
		patients.Columns.Add("Id", typeof(int));
		patients.Columns.Add("Name", typeof(String));
		patients.Columns.Add("Surname", typeof(String));
		patients.Columns.Add("Gender", typeof(String));
		patients.Columns.Add("DateOfBirth", typeof(String));
		patients.Columns.Add("Profession", typeof(String));
		patients.Columns.Add("City", typeof(String));
		patients.Columns.Add("Address", typeof(String));
		patients.Columns.Add("PhoneNumber", typeof(String));
		patients.Columns.Add("PathFile", typeof(String));
		string[] table = new string[patients.Columns.Count - 1];
		string[] cleared = new string[patients.Columns.Count];
		string[] s = null;
		bool check = true;
		Node node = root;
		int iterator = 0;
		while (root.getCounter() > 0)
		{
			var row = patients.NewRow();
			do
			{
				if (node.getNodes()[iterator].getCounter() > 0)
				{
					node.CounterMinusOne();
					node = node.getNodes()[iterator];
					int categ = node.getCategory();
					if (check)
					{
						table[categ] += node.getSign();
					}
					else
					{
						table[categ] += "*";
					}
					iterator = 0;
					if (node.isEnd())
					{
						node.CounterMinusOne();
						table[table.Length - 1] = node.getFromStack();
					}
					if (node.getSign().Equals('*'))
					{
						check = false;
					}
				}
				else
				{
					iterator++;
				}
			} while (!node.isEnd());
			check = true;
			iterator = 0;
			node = root;
			s = table[table.Length - 1].Split(";");
			cleared[0] = s[0];
			cleared[9] = s[1];
			for (int i = 1; i < cleared.Length - 1; i++)
			{
				cleared[i] = table[i - 1].Trim('*') + "*";
			}
			row["Id"] = int.Parse(cleared[0]);
			row["Name"] = cleared[1];
			row["Surname"] = cleared[2];
			row["Gender"] = table[2];
			row["DateOfBirth"] = cleared[4];
			row["Profession"] = cleared[5];
			row["City"] = cleared[6];
			row["Address"] = cleared[7];
			row["PhoneNumber"] = cleared[8];
			row["PathFile"] = cleared[9];
			if (patients.Rows.Count < 100)
			{
				Patient patient = new Patient(cleared[0], cleared[1], cleared[2], cleared[8], cleared[7], cleared[2], cleared[5],
				cleared[6], cleared[4]);
				row["PathFile"] = DOCXGen.generateDocument(patient) + ";" + JPGGen.generateDocument(patient) + ";" + DICOMGen.generateDocument(patient);
			}
			table = new string[patients.Columns.Count - 1];
			cleared = new string[patients.Columns.Count];

			s = null;
			patients.Rows.Add(row);
		}
		patients.DefaultView.Sort = "Id";

		return patients;
	}

	private void Inor(Node node)
	{
		if (node == null)
		{

			return;
		}
		Console.WriteLine(node.getSign() + " " + node.getCounter());
		for (int i = 0; i < node.getNodes().Count; i++)
		{
			Inor(node.getNodes()[i]);
		}
	}

	public void printIT()
	{
		Inor(root);
	}
}
