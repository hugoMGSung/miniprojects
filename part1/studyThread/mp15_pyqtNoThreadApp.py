# 스레드 미사용 앱
import sys
from PyQt5 import uic, QtCore, QtGui
from PyQt5.QtWidgets import *
from PyQt5.QtGui import * # QIcon은 여기 있음
from PyQt5.QtCore import * # Qt.white...

class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        uic.loadUi('./studyThread/threadApp.ui', self)
        self.setWindowTitle('노쓰레드앱 v0.4')
        self.pgbTask.setValue(0)

        self.btnStart.clicked.connect(self.btnStartClicked)

    def btnStartClicked(self):
        self.pgbTask.setRange(0, 999999) 
        for i in range(0, 1000000): # 0 ~ 100
            print(f'노스레드 출력 > {i}')
            self.pgbTask.setValue(i)
            self.txbLog.append(f'노스레드 출력 > {i}')

if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())