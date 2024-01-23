# -*- coding: utf-8 -*-

import RPi.GPIO as GPIO
import time
from typing import Any

class Ta7291PinGuide:
    in1 = None
    in2 = None
    pwm = None
	
    def checkAvailability(self):
        if self.in1 != None and self.in2 != None:
            return True
        else:
            return False

class Ta7291Single:
	def __init__(self, pin:Ta7291PinGuide):
		if pin.checkAvailability() == False:
			raise ValueError("the Value called pin not has apporopriate value")
		# Set up and back up gpio pin 
		GPIO.setmode(GPIO.BCM)
		GPIO.setup(pin.in1, GPIO.OUT)
		GPIO.setup(pin.in2, GPIO.OUT)
		self.in1 = pin.in1
		self.in2 = pin.in1
		self.p = None
		if pin.pwm != 0:
			GPIO.setup(pin.pwm, GPIO.OUT)
			self.p = GPIO.PWM(pin.pwm,50)
		

	def Drive(self, speed):
		if speed > 0:
			GPIO.output(self.in1, 1)
			GPIO.output(self.in2, 0)
			if self.p is None:
				self.p.start(speed)
			
		if speed < 0:
			GPIO.output(self.in1, 0)
			GPIO.output(self.in2, 1)
			if self.p is None:
				self.p.start(-speed)

		if speed == 0:
			GPIO.output(self.in1, 0)
			GPIO.output(self.in2, 0)
			
	def Brake(self):
		GPIO.output(self.in1, 1)
		GPIO.output(self.in2, 1)
		time.sleep(0.5)

	def CleanUp(self):
		self.brake()
		GPIO.cleanup()
		
class Ta7291Duo:
	_right = None
	_left = None
	def __init__(self, right:Ta7291PinGuide, left:Ta7291PinGuide):
		try:
			_right = Ta7291Single(right)
			_left = Ta7291Single(left)
		except:
			raise
	
	def Forward(self, speed):
		_right.Drive(abs(speed))
		_left.Drive(abs(speed))
	
	def Back(self, speed):
		_right.Drive(abs(speed) * -1)
		_left.Drive(abs(speed) * -1)

	def TurnRight(self, speed):
		_right.Drive(abs(speed) * -1)
		
	def TurnLeft(self, speed):
		_left.Drive(abs(speed) * -1)

	def TurnRightSharply(self, speed):
		_right.Drive(abs(speed))
		_left.Drive(abs(speed) * -1)
		
	def TurnLeftSharply(self, speed):
		_right.Drive(abs(speed) * -1)
		_left.Drive(abs(speed))
		
	def Brake(self):
		_right.Brake()
		_left.Brake()
		time.sleep(0.5)

	def Cleanup(self):
		_right.Cleanup()
		_left.Cleanup()
		
		

if __name__ == "__main__":
	pass
