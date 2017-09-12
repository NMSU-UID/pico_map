import pygame
import pygame.camera
import logging


def SetupCam():
    logging.info('Starting image record.')
    pygame.init()
    pygame.camera.init()

    try:
        cam = pygame.camera.Camera("/dev/video0", (640, 480))
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
