#include <iostream>
#include <vector>
#include <queue>
#include <time.h>
#include <unistd.h>
using namespace std;


class Receiver {
	vector<queue<int>> sqn;
public:
	Receiver() {
		for (int i = 0; i < 5; i++) {
			queue<int> temp;
			sqn.push_back(temp);
		}
	}
	void addRequestforResource(int rid, int reqno);
	//adds a new quest with number reqno to the resource sqn[rid]
	//rid is between 0 and 4, as we have 5 resources

	void serviceRequestatResource(int rid);

	//services the request (dequeues) at front of sqn[rid]
	void printQueues();
	//prints all queues line by line, numbers separated by spaces
	bool checkStop();
};

void service(Receiver& obj);

void service2(Receiver& obj);

int main() {

	srand(time(0));

	Receiver R1;

	service2(R1);
}

void Receiver::addRequestforResource(int rid, int reqno) {

	sqn[rid].push(reqno);
}

void Receiver::serviceRequestatResource(int rid) {

	if (!sqn[rid].empty())
		sqn[rid].pop();
}

void Receiver::printQueues() {

	system("clear");

	cout << endl << "Printing Requests" << endl << endl;

	queue<int> tempQueue;

	for (int i = 0; i < 5; i++) {

		while (!sqn[i].empty()) {

			tempQueue.push(sqn[i].front());
			cout << sqn[i].front() << " ";
			sqn[i].pop();
		}

		while (!tempQueue.empty()) {

			sqn[i].push(tempQueue.front());
			tempQueue.pop();
		}

		cout << endl;
	}
}

void service(Receiver& obj) {

	cout << "Starting Service" << endl << endl;

	int serviceRates[5] = {0,0,0,0,0};
	int timepassed = 0;
	int printTimePassed = 0;

	for (int j = 0; j < 5; j++) {

		do {

			cout << "Enter service Rate for resource " << j + 1 << " : ";
			cin >> serviceRates[j];

			if (serviceRates[j] >= 500) {
				cout << "service Rate is too high" << endl;
			}
			else if (serviceRates[j] <= 0) {
				cout << "service Rate can not be a negative number" << endl;
			}

		} while (serviceRates[j] <= 0 || serviceRates[j] >= 500);
	}

	int i = 0;
	bool flag = 1;

	while (flag) {

		if (i < 100) {

			obj.addRequestforResource(rand() % 5, i);
		}
		else {

			for (int j = 0; j < 5 && flag; j++) {

				obj.serviceRequestatResource(j);
				sleep(serviceRates[j]);
				timepassed += serviceRates[j];
				printTimePassed += serviceRates[j];


				if (printTimePassed >= 250) {
					obj.printQueues();
					printTimePassed -= 250;
				}

				if (timepassed >= 500) {
					obj.addRequestforResource(rand() % 5, i++);
					timepassed -= 500;
				}

				if (obj.checkStop()) {
					flag = 0;
				}
			}

		}

		i++;
	}
}

void service2(Receiver& obj) {

	cout << "Starting Service" << endl << endl;

	int serviceRates[5] = { 0,0,0,0,0 };
	int timepassed = 0;
	int printTimePassed = 0;

	for (int j = 0; j < 5; j++) {

		cout << "Enter service Rate for resource " << j + 1 << " : ";
		cin >> serviceRates[j];
	}

	int i = 0;
	bool flag = 1;

	while (flag) {

		if (i < 100) {

			obj.addRequestforResource(rand() % 5, i);
			i++;
		}
		else {

			timepassed++;
			sleep(1);

			for (int j = 0; j < 5; j++) {

				if (timepassed % serviceRates[j] == 0) {

					obj.serviceRequestatResource(rand() % 5);
				}
			}

			if (timepassed % 500 == 0) {

				obj.addRequestforResource(rand() % 5, i++);
			}

			if (timepassed % 250 == 0) {

				obj.printQueues();
			}
		}

		if (obj.checkStop()) {

			flag = 0;
		}
	}
}

bool Receiver::checkStop() {

	for (int i = 0; i < 5; i++) {

		if (!sqn[i].empty())
			return 0;
	}
	return 1;
}