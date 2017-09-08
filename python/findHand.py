import cv2

# static set image for testing
img = cv2.imread('handTest.png')

# Change image to grayscale
# !!Video frame will also need to be grayscale
gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

# Apply a gausian blur to the gray image
gray = cv2.GaussianBlur(gray, (19, 19), 0)

# Apply a threshold to the image
ret, grayThresh = cv2.threshold(gray, 0, 255,
                                cv2.THRESH_BINARY_INV + cv2.THRESH_OTSU)

# Find all contours
contours, heirarchy = cv2.findContours(grayThresh,
                                       cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

# Draw the contours
cv2.drawContours(img, contours, -1, (0, 255, 0), 3)

# show the original image with contours drawn on
cv2.imshow('original', img)
cv2.waitKey(0)
cv2.destroyAllWindows()

# Next step: identify fingers, try and isolate
# a hand from background noise (left green bar)
