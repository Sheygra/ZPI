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
		this.counter++;
	}

	public Node(char sign, Node parent)
	{
		this.sign = sign;
		this.counter++;
		this.parent = parent;
	}

	public Node(char sign, Node parent, int category)
	{
		this.sign = sign;
		this.counter++;
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
		if(stack == null)
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
			while (data.Length > patients.Columns.Count-2)
			{
				for (int k = 0; k < data.Length - 1; k++)
				{
					if (data[k] == ';')
					{
						if ((!data[k + 1].Equals(';')))
						{
							for (int j = 0; j < node.getNodes().Count; j++)
							{
								if (node.getNodes()[j].getSign() == data[k + 1])
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
			node.addToStack(patients.Rows[i].ItemArray[0]+";"+patients.Rows[i].ItemArray[4]);
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
					nodeStack.Push(node.getNodes()[i]);
				}
				else nodeStack.Push(node.getNodes()[i]);
			}
		}
	}

	//Getting data back into DataTable
	public DataTable normalize()
	{
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
		string[] table = new string[patients.Columns.Count-1];
		string[] s = null;
		Node node = root;
		int iterator = 0;
		while (root.getCounter()>0)
		{
			var row = patients.NewRow();
			do
			{
				if (node.getNodes()[iterator].getCounter() > 0)
				{
					node.CounterMinusOne();
					node = node.getNodes()[iterator];
					int categ = node.getCategory();
					table[categ] += node.getSign();
					iterator = 0;
					if (node.isEnd())
					{
						node.CounterMinusOne();
						table[table.Length-1] = node.getFromStack();
					}
				}
				else
				{
					iterator++;
				}
			} while (!node.isEnd());
			iterator = 0;
			node = root;
			row["Name"] = table[0].Trim('*')+"*";
			row["Surname"] = table[1].Trim('*') + "*";
			row["Gender"] = table[2];
			row["DateOfBirth"] = table[3].Trim('*') + "*";
			row["Profession"] = table[4].Trim('*') + "*";
			row["City"] = table[5].Trim('*') + "*";
			row["Address"] = table[6].Trim('*') + "*";
			row["PhoneNumber"] = table[7].Trim('*') + "*";
			
			s = table[table.Length-1].Split(";");
			row["Id"] = int.Parse(s[0]);
			row["PathFile"] = s[1];
			table = new string[patients.Columns.Count-1];
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
		Console.WriteLine(node.getSign() + " "+ node.getCounter());
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
