import pygame
import pygame.camera
import logging


def recordImages():
    logging.info('Starting image record.')
    pygame.init()
    pygame.camera.init()

    try:
        cam = pygame.camera.Camera("/dev/video0", (640, 480))
        cam.start()
    except Exception, e:
        print "Error connecting to camera."
        print e

    print "Getting image."
    try:
        image = cam.get_image()
    except Exception, e:
        print "Error getting image"
        print e
        return

    logging.info("Saving image.")
    try:
        pygame.image.save(image, 'image.jpg')
    except Exception, e:
        print "Error saving image:"
        print e
