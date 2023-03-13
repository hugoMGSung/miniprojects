# QRCODE PyQT app
import sys
from PyQt5 import uic, QtCore, QtGui
from PyQt5.QtWidgets import *
from PyQt5.QtGui import * # QIcon은 여기 있음
from PyQt5.QtCore import * # Qt.white...
import qrcode

class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        uic.loadUi('./studyPython/qrcodeApp.ui', self)
        self.setWindowTitle('Qrcode 생성앱 v0.1')
        self.setWindowIcon(QIcon('./studyPython/qr-code.png'))

        # 시그널/슬롯
        self.btnQrGen.clicked.connect(self.btnQrGenClicked)
        self.txtQrData.returnPressed.connect(self.btnQrGenClicked)

    def btnQrGenClicked(self):
        data = self.txtQrData.text()

        if data == '':
            QMessageBox.warning(self, '경고', '데이터를 입력하세요')
            return
        else:
            qr_img = qrcode.make(data)
            qr_img.save('./studyPython/site.png')

            img = QPixmap('./studyPython/site.png')            
            self.lblQrCode.setPixmap(QPixmap(img).scaledToWidth(300))

if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())