using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Security.Permissions;
using System.Data;
using System.Globalization;
using System.Linq;
using ZPI_Projekt_Anonimizator.entity;

class Node
{
	List<Node> nodes = new List<Node>(0);
	char sign;
	Node parent;
	int counter;
	bool end;
	int category;

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
}

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



	public void add(DataTable patients)
	{
		string data = "";
		int category = 0;
		Node node = root;
		bool check = false;
		for (int i = 0; i < patients.Rows.Count; i++)
		{
			foreach (string s in patients.Rows[i].ItemArray)
			{
				data += ";" + s;
			}
			category = 0;
			slowa++;
			node = root;
			check = false;
			while (data.Length > 8)
			{
				for (int k = 0; k < data.Length - 1; k++)
				{
					if (data[k] == ';')
					{
						category++;
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
					}
				}
				category = 0;
			}
			node.setEnd(true);
			data = "";
		}
	}

	public void k_anonymization(int k)
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
					node.setEnd(true);
				}
				else nodeStack.Push(node.getNodes()[i]);
			}
		}
	}

	public Patient normalize(Patient patient)
	{
		Node node = root;
		while (node.getNodes() != null)
		{
			for (int i = 0; i < node.getNodes().Count; i++)
			{

			}
		}
		return patient;
	}

	private void Inor(Node node)
	{
		if (node == null)
		{
			return;
		}
		Console.WriteLine(node.getSign() + " " + node.getCategory() + " " + node.isEnd() + node.getCounter());
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
