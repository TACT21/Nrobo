import subprocess
import os

subprocess.run([os.path.dirname(os.path.abspath(__file__)) + "/installer.sh",""], shell=True)

https://www.mikan-tech.net/entry/raspi-wifi-ap