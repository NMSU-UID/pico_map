'''
Pico Map
Cyrille Gindreau
2017

Module for interfacing with a serial camera using pygame

Methods:
PollDev()
    scans /dev for any new devices. Scan rate is set at 1 minute
    once found, will return the name of the file.
    NOTE: the way devices are named is greatly dependent on operating
    system, I've tested on OSX and raspberry pis but this should be
    updated for other systems as we find them.

SetupCam()
    params: the device name from the dev
    creates a pygame camera object, raises exception if
    device is not a camera.

CaptureImage()
    params: a pygame camera.
    User the pygame camera to take a picute and save it.

GetImageRaw()
    params a pygame camera
    Same as Capture Image except it returns a raw string.

Main()
    Runs through all the methods except GetImageRaw
    This should be used only as a test to make sure the camera
    is operating correctly. Also serves to take test pictures.

TODO:
    Altough Pygame is simple and accessible, it is based off
    the opencv library. We should at some point convert this to
    OpenCV.

'''
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


def SetupCam(camera):
    print 'Setting up camera.'
    pygame.init()
    pygame.camera.init()

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
    print "finding devices."
    camera = PollDev()
    print "Setting up."
    try:
        cam = SetupCam(camera)
    except Exception, e:
        print "Error setting up the camera."
        exit()
    try:
        CaptureImage(cam)
    except Exception, e:
        print "Error capturing image."
        print e
        exit()
