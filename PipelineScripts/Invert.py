from sys import argv
with open(argv[1]) as f:
	for line in f.readlines():
		print(line[::-1], end = '')