# Change transparent pixels to have the same color as neighboring opaque pixels
# Usage examples:
# python fix_border_pixels.py foo.png # Outputs to foo-fixed.png by default
#
# Fix up all images within a directory (recursive)
# find Assets/Images -name \*.png -print0 | xargs -0 python match_transparent_pixels.py

# name modified files with a label -- set to the empty string to overwrite original files
label = '-fixed'

# Overwrite existing changed images?
overwrite = False

import os
import os.path
import sys

try:
    from PIL import Image
except ImportError:
    print "PIL not found."
    print "Check your python installation or download PIL from http://www.pythonware.com/products/pil/"
    sys.exit(1)

# transparency threshold. Set to 254 to view changed transparent pixels.
transparent = 0

pen_down = False

def correct_color(pix, size, current, previous):
    global pen_down
    width, height = size

    x1, y1 = previous
    if x1 >= width or x1 < 0 or y1 < 0 or y1 >= height:
        pen_down = False
        return
    r1,g1,b1,a1 = pix[x1, y1]

    if a1 > transparent:
        pen_down = True

    if pen_down:
        x, y = current
        pix[x,y] = (r1,g1,b1,transparent)

def new_image_name(filename):
    global label
    base, ext = os.path.splitext(filename)
    return base + label + ext

sys.stdout.write("Correcting Images ")
for f in sys.argv[1:]:
    if os.path.exists(new_image_name(f)) and not overwrite:
        continue

    sys.stdout.write('.')
    im = Image.open(f)

    # Only makes sense for RGBA
    if im.mode != 'RGBA':
        continue

    width, height = im.size
    pix = im.load()

    # Scan in each direction filling in colors
    for x in range(0, width):
        for y in range(0, height):
            if pix[x,y][3] <= transparent:
                correct_color(pix, im.size, (x, y), (x, y-1))
        for y in range(height, 0, -1):
            y -= 1
            if pix[x,y][3] <= transparent:
                correct_color(pix, im.size, (x, y), (x, y+1))
    for y in range(0, height):
        for x in range(0, width):
            if pix[x,y][3] <= transparent:
                correct_color(pix, im.size, (x, y), (x-1, y))
        for x in range(width, 0, -1):
            x -= 1
            if pix[x,y][3] <= transparent:
                correct_color(pix, im.size, (x, y), (x+1, y))


    #im.show()
    im.save(new_image_name(f))

print ""
