from sys import argv
from PIL import Image
img = Image.open(argv[1])
img = img.convert("L")
img.save(argv[1])