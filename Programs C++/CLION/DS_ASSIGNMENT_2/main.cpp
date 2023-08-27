#include <iostream>
#include <vector>
#include <unordered_map>
using namespace std;

void solve(string s, unordered_map<int,vector<char>>&m, vector<string>&ans, string curr,int idx) {
	if(idx >= s.size()) {
		ans. push_back(curr);
		return ;
	}
	string res;
	char c= s[idx];
	int v=c-'0';
	for(auto i: m[v]) {
		solve(s, m, ans, curr + i, idx + 1);
	}
}

vector<string> letterCombinations(string s) {
	vector<string>ans;
	if(!s.size())
		return ans;
	unordered_map<int, vector<char>>m;
	char c = 'a';
	for(int i = 2; i <=9; ++i) {
		int ch = 3;
		if (i == 7 || i == 9)
			ch++;
		while (ch--) {
			m[i].push_back(c);
			c++;
		}
	}
		solve(s, m, ans,"",0);
		return ans;
}


int main(){
	return 0;
}

//All in one header file  using namespace std;
// vector<string> res;
// vector to store strings map<char,string>
// mp={{'2',"ABC"},{'3',"DEF"},{'4',"GHI"},{'5',"JKL"},{'6',"MNO"},{'7',"PQRS"},{'8',"TUV"},{'9',"WXYZ"}};
// void lettercombo(string &s, int pos, string &v){
// if(pos>=s.length())          //base case     {
// res.push_back(v);        //storing our valid string
// }
// for(int i=0;i<mp[s[pos]].length();i++)     {         //**BACKTRACKING**
// v.push_back(mp[s[pos]][i]);
// do         lettercombo(s,pos+1,v);        //recur
// v.pop_back();                  //undo
// }
// }
// bool letterCombinations(string digits) {
//  if(digits.length()>=2&&digits.length()<=8){
// checking if string length is greater than equal to 2 and less than equal to 8
// string v;
// lettercombo(digits,0,v);
// return true;     }
// return false;
// }
//x
// int main() {
// string digits;
// cout<<"INPUT: ";
// cin>>digits;
// cout<<"\nOUTPUT:\n";
// bool flag = true;
// for(auto &x: digits){
// if(x=='1'||x=='*'||x=='0'||x=='#'){
// checking if our input string is valid
// cout<<"INVALID INPUT!!!";
// flag = false;         }
// }
// if(flag){
// bool ans = letterCombinations(digits);
// if(ans){
// for(auto &x: res)
// cout<<x<<" ";
// }
// else cout<<"INVALID INPUT!!!";
// }
// }

