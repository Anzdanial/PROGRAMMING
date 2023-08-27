# Prims algorithm. ideal is: O(E log_e(V)), but this one is O(V^2)
# Prims will work for both positive & negative weights, undirected
G = [[0, 9, 75, 0, 0],
		 [9, 0, 95, 19, 42],
		 [75, 95, 0, 51, 66],
		 [0, 19, 51, 0, 31],
		 [0, 42, 66, 31, 0]]
selected = [0, 0, 0, 0, 0]
selected[0] = True
print("Edge : Weight\n")
for _ in range(0, len(G)):
	minimum = 9999999
	x = -1
	y = -1
	for i in range(len(G)):
		if not selected[i]:
			continue
		for j in range(len(G)):
			if i != j and (not selected[j]) and G[i][j] and minimum > G[i][j]:
				minimum = G[i][j]
				x = i
				y = j
	if x == -1 or y == -1:
		break;
	print(str(x) + "-" + str(y) + ":" + str(G[x][y]))
	selected[y] = True
