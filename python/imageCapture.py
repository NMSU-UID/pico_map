import pygame
import pygame.camera
import logging
from os import walk
import time

# For use on rasberry pi
USBPREFIX = 'ttyUSB'

# Foruse on OSX
# USBPREFIX = 'tty.usb'


def PollDev():
    pollRate = 60
    deviceFound = False
    while(deviceFound):
        f = []
        for (dirpath, dirnames, filenames) in walk("/dev/"):
            f.extend(filenames)
        for i in f:
            if i.startswith(USBPREFIX):
                print "New device found, starting serial: {}".format(i)
                return i
        time.sleep(pollRate)


def SetupCam():
    print 'Setting up camera.'
    pygame.init()
    pygame.camera.init()
    camera = PollDev()

    try:
        cam = pygame.camera.Camera("/dev/{}".format(camera), (640, 480))
        cam.start()
        return cam
    except Exception, e:
        print "Error connecting to camera."
        raise Exception(e)


def CaptureImage(cam):
    print "Getting image."
    try:
        image = cam.get_image()
    except Exception, e:
        print "Error getting image"
        raise Exception(e)

    logging.info("Saving image.")
    try:
        pygame.image.save(image, 'image.jpg')
    except Exception, e:
        print "Error saving image:"
        print e
        raise Exception(e)


def GetImageRaw(cam):
    print "Getting image."
    try:
        return cam.get_raw()
    except Exception, e:
        print "Error getting image string."
        raise Exception(e)


if __name__ == "__main__":
    print "Setting up."
    try:
        cam = SetupCam()
    except Exception, e:
        print "Error setting up the camera."
        exit()
    try:
        CaptureImage(cam)
    except Exception, e:
        print "Error capturing image."
        print e
        exit()
