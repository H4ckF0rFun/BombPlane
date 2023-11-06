#include <iostream>
#include <vector>
#include <cstring>
#include <set>

using namespace std;

#define N 10    // 地图边长
#define P_NUM 3  // 飞机数目

typedef pair<int, int> pos;

enum mapType {
	unknown,
	empty,
	plane,
	planeHead,
};

struct node {
	mapType map[N + 1][N + 1] = { mapType::empty };

	long long hash() {
		long long M = 10000000000037, p = 107;
		long long ret = 1;
		for (int i = 1; i <= N; i++)
			for (int j = 1; j <= N; j++)
				if (map[i][j] == planeHead || map[i][j] == plane)
					ret = ret * (i * N + j) % M * p % M;
		return ret;
	}
};

void printNode(node x) {
	for (int i = 1; i <= N; i++) {
		for (int j = 1; j <= N; j++) {
			if (x.map[i][j] == mapType::empty) cout << "_";
			else if (x.map[i][j] == mapType::plane) cout << "*";
			else if (x.map[i][j] == mapType::planeHead) cout << "&";
			cout << " ";
		}
		cout << endl;
	}
}


mapType a[N + 1][N + 1] = { mapType::empty };
int upPlane[10][2] = { +1, -2, +1, -1, +1, 0, +1, +1, +1, +2, +2, 0, +3, -1, +3, 0, +3, +1 };
int downPlane[10][2] = { -1, -2, -1, -1, -1, 0, -1, +1, -1, +2, -2, 0, -3, -1, -3, 0, -3, +1 };
int leftPlane[10][2] = { -2, +1, -1, +1, 0, +1, +1, +1, +2, +1, 0, +2, -1, +3, 0, +3, +1, +3 };
int rightPlane[10][2] = { -2, -1, -1, -1, 0, -1, +1, -1, +2, -1, 0, -2, -1, -3, 0, -3, +1, -3 };

void dfs(int nowPNum, vector<node>& VN) {
	if (nowPNum > P_NUM) {
		node x;
		// copy(begin(a), end(a), begin(x.map));
		memcpy(x.map, a, sizeof(a));
		VN.push_back(x);
		return;
	}
	/**
	 * 枚举当前这架飞机的所有可能位置
	 * 总共4种情况，即对应飞机的4种朝向，每种情况枚举机头位置
	 */
	mapType b[N + 1][N + 1];
	memcpy(b, a, sizeof(a));   // 备份数组a

	for (int dir = 0; dir < 4; dir++)   // 枚举飞机朝向
		for (int i = 1; i <= N; i++)
			for (int j = 1; j <= N; j++) {  // 枚举机头位置
				memcpy(a, b, sizeof(b));   // 首先初始化数组a
				bool flag = true;
				if (a[i][j] != mapType::empty) continue;
				a[i][j] = planeHead;
				for (int k = 0; k < 9; k++) {   // 枚举机身位置
					int ii, jj;
					if (dir == 0) ii = i + upPlane[k][0], jj = j + upPlane[k][1];
					else if (dir == 1) ii = i + downPlane[k][0], jj = j + downPlane[k][1];
					else if (dir == 2) ii = i + leftPlane[k][0], jj = j + leftPlane[k][1];
					else ii = i + rightPlane[k][0], jj = j + rightPlane[k][1];
					if (ii < 1 || ii > N || jj < 1 || jj > N) {
						flag = false;
						break;
					};
					if (a[ii][jj] != mapType::empty) {
						flag = false;
						break;
					};
					a[ii][jj] = plane;
				}
				if (flag) dfs(nowPNum + 1, VN);
			}
}

vector<node> initNodes() {
	/**
	 * 给出所有可能的摆放方式（已去重）
	 */
	for (int i = 1; i <= N; i++)
		for (int j = 1; j <= N; j++)
			a[i][j] = mapType::empty;

	vector<node> temp, ret;
	dfs(1, temp);
	set<long long> s;   // 用于去重
	for (node x : temp) {
		long long h = x.hash();
		if (!s.count(h)) {
			ret.push_back(x);
			s.insert(h);
		}
	}
	return ret;
}

pos getNextStep(const vector<node>& s, mapType nowMap[N + 1][N + 1]) {
	int ii = 0, jj = 0, maxEarn = 0;
	for (int i = 1; i <= N; i++)
		for (int j = 1; j <= N; j++)
			if (nowMap[i][j] == unknown) {  // 枚举可以选的位置
				// 首先计算当前位置各种情况的频率
				int p1 = 0, p2 = 0, p3 = 0;
				for (node x : s) {
					if (x.map[i][j] == mapType::planeHead) p2++;
					if (x.map[i][j] == mapType::plane) p1++;
					if (x.map[i][j] == mapType::empty) p3++;
				}
				int earn = p3 * (p1 + p2) + p2 * (p1 + p3) + p1 * (p2 + p3);
				if (earn > maxEarn) {
					ii = i, jj = j;
					maxEarn = earn;
				}
			}
	return make_pair(ii, jj);
}

void elimination(vector<node>& s, int x, int y, mapType m) {
	vector<node> temp;
	temp.reserve(s.size());
	for (node t : s) temp.push_back(t);
	s.clear();
	for (node t : temp) {
		if (t.map[x][y] == m) s.push_back(t);
	}
}

int main() {
	int key = 0;
	vector<node> s = initNodes();
	mapType nowMap[N + 1][N + 1] = { unknown };
	int tot = 0;
	char c0;
	int A[102][5] = { 0 };
	int x0 = 1, y0 = 1, res0 = 0;
	for (int i = 1; i <= 10; i++) {
		for (int j = 1; j <= 10; j++) {

			c0 = getchar();
			if (c0 == ' ')res0 = 0;
			else if (c0 == 'B')res0 = 1;
			else if (c0 == 'H')res0 = 2;
			else res0 = -1;
			if (res0 >= 0) {
				A[x0][1] = i;
				A[x0][2] = j;
				A[x0][3] = res0;
				x0++;
			}
		}
	}
	pos p;
	while (s.size() > 1 && tot < P_NUM) {
		p = getNextStep(s, nowMap);
		if (y0 == x0) {
			cout << p.second - 1 << '\n' << p.first - 1 << endl;
			return 0;
		}

		int x, y, res;
		do {

			x = A[y0][1];
			y = A[y0][2];
			res = A[y0][3];
		} while (x < 1 || x > N || y < 1 || y > N || res < 0 || res > 2);
		y0++;
		if (res == 0) nowMap[x][y] = mapType::empty, elimination(s, x, y, mapType::empty);
		else if (res == 1) nowMap[x][y] = mapType::plane, elimination(s, x, y, mapType::plane);
		else if (res == 2) nowMap[x][y] = mapType::planeHead, elimination(s, x, y, mapType::planeHead), tot++;
	}
	if (tot < P_NUM) {
		// 最后一步
		node x = *s.begin();
		for (int i = 1; i <= N; i++)
			for (int j = 1; j <= N; j++)
				if (x.map[i][j] == mapType::planeHead && nowMap[i][j] == unknown) {
					cout << j - 1 << '\n' << i - 1;
					nowMap[i][j] = mapType::planeHead;
					tot++;
					key = 1;
					return 0;
				}
	}



	return 0;
}