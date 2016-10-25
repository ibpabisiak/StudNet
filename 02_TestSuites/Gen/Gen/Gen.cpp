// Gen.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <string>
#include <iostream>

#include <fstream>
#include <vector>

int LoadTestingData(std::vector<std::vector<std::string>> &DataFile, std::vector<std::string> &ParametrDataFile, std::string NameDataFile)
{
	int lp,lt;
	std::ifstream Read(NameDataFile);
	if (Read.is_open())
	{
		Read >> lp;
		Read >> lt;
		std::string tmp;
		DataFile.resize(lt);
		int i = 0;
		while (!Read.eof())
		{
			Read >> tmp;
			if (i<lp)
			{
				ParametrDataFile.push_back(tmp);
			}
			else
			{
				if (tmp != "<empty>")
				{
					DataFile[(i - lp) / lp].push_back(tmp);
				}
				else
				{
					DataFile[(i - lp) / lp].push_back("");
				}
			}	
			i++;
		}
	}

	return 0;
}

void GenMfile(std::string TestName,int lt)
{
	std::ofstream Out(TestName);
	std::ifstream Read("man");
	int i = 0;
	if (Read.is_open())
	{
		std::string tmp;
		while (!Read.eof())
		{
			std::getline(Read, tmp);
			Out << tmp << std::endl;
			if (i==9)
			{
				
				
				for (int k = 0; k < lt; k++)
				{
					std::string tmpn;
					sprintf((char*)tmpn.c_str(), "%d", k);
					std::string str = tmpn.c_str();
					std::string WriteName = TestName + str + ".html";
					Out << "<tr><td><a href=\"" << WriteName << "\">" << TestName + str << "</a></td></tr>" << std::endl;
				}
			}
			i++;
		}
	}
}

void Generate(std::vector<std::vector<std::string>> DataFile, std::vector<std::string> ParametrDataFile, std::string TestName, std::string ExampleTestFile)
{
	std::ifstream Read(ExampleTestFile);
	std::vector<std::string > Exampl;
	if (Read.is_open())
	{
		while (!Read.eof())
		{
			std::string tmp;
			std::getline(Read, tmp);
			Exampl.push_back(tmp);
		}
	}
	for (int i = 0; i < DataFile.size(); i++)
	{
		std::string tmp;
		sprintf((char*)tmp.c_str(), "%d", i);
		std::string str = tmp.c_str();
		std::string WriteName = TestName + str + ".html";

		std::ofstream Out(WriteName);
		for (int j = 0; j < Exampl.size(); j++)
		{
			Out << Exampl[j] << std::endl;
			for (int k = 0; k < ParametrDataFile.size(); k++)
			{
				if (Exampl[j].find(ParametrDataFile[k]) != std::string::npos)
				{
					Out << "<td>" << DataFile[i][k] << "</td>" << std::endl;
					j++;
					break;
				}
			}
		}
	}
	GenMfile(TestName, DataFile.size());
}

int main()
{
	std::string TestName,ExampleTestFile,NameDataFile;
	std::cout << "Podaj nazwe testu: ";
	std::cin >> TestName;
	std::cout << "Podaj nazwe pliku przyk³adowego testu: ";
	std::cin >> ExampleTestFile;
	std::cout << "Podaj nazwe pliku z danymi do testu: ";
	std::cin >> NameDataFile;
	std::cout << std::endl;
	std::vector<std::vector<std::string>> DataFile;
	std::vector<std::string> ParametrDataFile;
	LoadTestingData(DataFile, ParametrDataFile, NameDataFile);
	//Generate(DataFile, ParametrDataFile, TestName, ExampleTestFile);


    return 0;
}

