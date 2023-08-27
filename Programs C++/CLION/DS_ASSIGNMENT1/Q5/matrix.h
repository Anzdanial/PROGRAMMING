#include <vector>
#include <iostream>
#include <fstream>
#include <string.h>

using namespace std;

class Matrix{
private:
	class Node{
	public:
		int value;
		int col;
		Node *next;
	};
	vector<Node*> rows;

	int width, height;

public:
	Matrix(){
		width = height = 0;
	}
	Matrix(int x, int y){
		width = x;
		height = y;
		for (int i = 0; i < height; i ++)
			rows.push_back(nullptr);
	}
	Matrix(string s){
		width = height = 0;
		read(s);
	}
	~Matrix(){
		for (auto head : rows){
			auto current = head;
			while (current){
				auto next = current->next;
				delete current;
				current = next;
			}
		}
		rows.clear();
	}
	Matrix(const Matrix& copy){
		for (auto head : copy.rows){
			auto current = head;
			if (current == nullptr){
				rows.push_back(nullptr);
			}else{
				Node* node = new Node(*current);
				rows.push_back(node);
				while (node->next){
					node->next = new Node(*(node->next));
					node = node->next;
				}
			}
		}
		width = copy.width;
		height = copy.height;
	}

	Matrix& operator=(const Matrix &copy){
		for (auto head : rows){
			auto current = head;
			while (current){
				auto next = current->next;
				delete current;
				current = next;
			}
		}
		rows.clear();
		for (auto head : copy.rows){
			auto current = head;
			if (current == nullptr){
				rows.push_back(nullptr);
			}else{
				Node* node = new Node(*current);
				rows.push_back(node);
				while (node->next){
					node->next = new Node(*(node->next));
					node = node->next;
				}
			}
		}
		width = copy.width;
		height = copy.height;
		return *this;
	}

	int getWidth() const {
		return width;
	}

	int getHeight() const {
		return height;
	}

	void read(string s){
		ifstream fin(s);
		if (!fin)
			return;
		width = height = 0;
		int colcount = 0;
		Node* current = nullptr;
		while (! fin.eof()){
			char c;
			fin.get(c);
			if (c >= '0' && c <= '9'){
				// time to read this till the end
				int num = c - 48;
				while (! fin.eof()){
					fin.get(c);
					if (fin.eof() || c < '0' || c > '9'){
						if (num != 0){
							if (current == nullptr){
								current = new Node;
								current->value = num;
								current->next = nullptr;
								current->col = colcount;
								rows.push_back(current);
								height ++;
							}else{
								current->next = new Node;
								current = current->next;
								current->value = num;
								current->next = nullptr;
								current->col = colcount;
							}
						}
						colcount ++;
						if (c == '\n' || c == '\r'){
							if (width == 0){
								width = colcount;
							}else if (colcount != width){
								fin.close();
								return;
							}
							colcount = 0;
							current = nullptr;
						}
						break;
					}
					num = (num * 10) + (c - 48);
				}
			}
		}
		fin.close();
	}

	void print() const {
		for (int R = 0; R < getHeight(); R ++){
			for (int C = 0; C < getWidth(); C ++){
				std::cout << getEntry(R, C) << "\t";
			}
			std::cout << "\n";
		}
		std::cout << "\n";
	}

	//Only stored values i.e abstracted stuff.
	void printRaw() const {
		for (auto head : rows){
			Node* current = head;
			while (current){
				cout << "col: " << current->col << "=" << current->value << "  ";
				current = current->next;
			}
			cout << "\n";
		}
	}

	int getEntry(int row, int col) const {
		Node* head = rows[row];
		while (head){
			if (head->col == col){
				return head->value;
			}else if (head->col > col){
				return 0;
			}
			head = head->next;
		}
		return 0;
	}

	void setEntry(int row, int col, int entry){
		if (rows[row] == nullptr){
			if (entry != 0){
				Node* head = new Node();
				head->value = entry;
				head->col = col;
				head->next = nullptr;
				rows[row] = head;
			}
		}else{
			// find place to shove it
			if (rows[row]->col == col){
				if (entry == 0){
					Node* head = rows[row];
					rows[row] = head->next;
					delete head;
				}else{
					rows[row]->value = entry;
				}
			}else{
				Node* head = rows[row];
				while (head->next && head->next->col < col)
					head = head->next;
				if (head->next){
					if (head->next->col == col){
						if (entry == 0){
							Node* todelete = head->next;
							head->next = todelete->next;
							delete todelete;
						}else{
							head->next->value = entry;
						}
					}else if (entry != 0){
						Node* newNode = new Node();
						newNode->value = entry;
						newNode->col = col;
						newNode->next = head->next;
						head->next = newNode;
					}
				}else if (entry != 0){
					Node* newNode = new Node();
					newNode->value = entry;
					newNode->col = col;
					newNode->next = nullptr;
					head->next = newNode;
				}
			}
		}
	}

	Matrix operator+(const Matrix& obj) const {
		if (width != obj.width || height != obj.height)
			return {};
		Matrix resultant(width, height);
		for (int R = 0; R < width; R ++){
			for (int C = 0; C < height; C ++)
				resultant.setEntry(R, C, obj.getEntry(R, C) + getEntry(R, C));
		}
		return resultant;
	}

	Matrix inverse() const {
		if (!(width == 2 && height == 2))
			return {};
		float determinant = getEntry(0,0) * getEntry(1, 1) -
		                    getEntry(0,1) * getEntry(1,0);
		Matrix resultant(2,2);
		resultant.setEntry(0, 0, getEntry(1, 1) / determinant);
		resultant.setEntry(1, 1, getEntry(0, 0) / determinant);
		resultant.setEntry(0, 1, -getEntry(0, 1) / determinant);
		resultant.setEntry(1, 0, -getEntry(1, 0) / determinant);
		return resultant;
	}

	Matrix getSubMatrix(int r, int c, int w, int h) const {
		if (r + h > height || c + w > width)
			return {};
		Matrix sub(w, h);
		for (int i = 0; i < h; i ++){
			for (int j = 0; j < w; j ++)
				sub.setEntry(i, j, getEntry(r + i, c + j));
		}
		return sub;
	}

	bool operator==(const Matrix& mat) const {
		if (mat.width != width || mat.height != height)
			return false;
		for (int i =0; i < height; i ++){
			for (int j = 0; j < width; j ++){
				if (getEntry(i, j) != mat.getEntry(i, j))
					return false;
			}
		}
		return true;
	}

	bool submatrix(const Matrix&mat) const{
		for (int i = 0; i < height; i ++){
			for (int j = 0; j < width; j ++){
				if (mat == getSubMatrix(i, j, mat.width, mat.height))
					return true;
			}
		}
		return false;
	}

	Matrix transpose() const{
		Matrix tr(height, width);
		for (int i = 0; i < height; i ++){
			for (int j = 0; j < width; j ++)
				tr.setEntry(j, i, getEntry(i, j));
		}
		return tr;
	}

	bool symmetric()const{
		if (transpose() == *this)
			return 1;
		return false;
	}
	bool skew()const{
		Matrix tr = transpose();
		for (int i = 0; i < tr.getWidth(); i ++){
			for (int j = 0; j < tr.getHeight(); j ++)
				tr.setEntry(i, j, tr.getEntry(i, j) * -1);
		}
		if (tr == *this)
			return true;
		return false;
	}

	Matrix operator*(const Matrix& mat)const {
		Matrix mult(mat.width, height);
		if (width != mat.height)
			return {};
		for(int i = 0; i < mult.getHeight(); i ++){
			for (int j = 0; j < mult.getWidth(); j ++){
				int sum = 0;
				for (int k=0; k < width; k ++)
					sum += getEntry(i, k) * mat.getEntry(k, j);
				mult.setEntry(i, j, sum);
			}
		}
		return mult;
	}
};