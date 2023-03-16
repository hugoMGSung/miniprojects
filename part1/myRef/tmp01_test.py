# Qt Designer 디자인 사용
import sys
from PyQt5 import uic
from PyQt5.QtWidgets import *

class qtApp(QWidget):
    count = 0 # 클릭횟수 카운트 변수

    def __init__(self):
        super().__init__()
        uic.loadUi('./myRef/test.ui', self)

if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())