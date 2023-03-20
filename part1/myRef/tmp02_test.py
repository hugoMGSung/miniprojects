# Qt Designer 디자인 사용
import sys
from PyQt5 import uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *

class qtApp(QWidget):
    count = 0 # 클릭횟수 카운트 변수

    def __init__(self):
        super().__init__()
        uic.loadUi('./myRef/buttonSample.ui', self)

    def resizeEvent(self, event) -> None:
        win_width = self.frameGeometry().width()
        win_height = self.frameGeometry().height()
        if win_width > 600:
            self.pushButton.setFont(QFont('나눔고딕', 20))
        if win_width > 1000:
            self.pushButton.setFont(QFont('나눔고딕', 40))
        

if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())