# -*- coding: utf-8 -*-

import RPi.GPIO as GPIO
import time
from typing import Any

class hc_sr04:
    def __init__(self, Triger, Echo):
		# Set up and back up gpio pin
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(Triger, GPIO.OUT)
        GPIO.setup(Echo, GPIO.IN)
        self.triger = Triger
        self.echo = Echo

    def ReadDistance(self):
        GPIO.output(self.triger, GPIO.HIGH)
        time.sleep(0.00001)
        GPIO.output(self.triger, GPIO.LOW)

        while GPIO.input(self.echo) == GPIO.LOW:
            sig_off = time.time()
        while GPIO.input(self.echo) == GPIO.HIGH:
            sig_on = time.time()

        duration = sig_off - sig_on
        distance = duration * 34000 / 2
        return distance
    
    def Cleanup(self):
        GPIO.cleanup()