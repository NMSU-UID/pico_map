import cv2

img = cv2.imread('handTest.png', 0)
#Test image is grayscale.  If non grayscale, image must be turned to grayscale.
ret, img = cv2.threshold(img, 0, 255, cv2.THRESH_BINARY_INV+cv2.THRESH_OTSU)
cv2.imshow('testImage', img)
cv2.waitKey(0)
cv2.destroyAllWindows();
