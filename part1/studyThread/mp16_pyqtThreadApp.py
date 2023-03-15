# 스레드 사용 앱
import sys
from PyQt5 import uic, QtCore, QtGui
from PyQt5.QtWidgets import *
from PyQt5.QtGui import * # QIcon은 여기 있음
from PyQt5.QtCore import * # Qt.white...
import time

MAX = 1000

class BackgroundWorker(QThread): # PyQt5 스레드를 위한 클래스 존재
    procChanged = pyqtSignal(int) # 커스텀 시그널(마우스 클릭같은 시그널을 따로 만드는 것)

    def __init__(self, count=0, parent=None) -> None:
        super().__init__()
        self.main = parent
        self.working = False # 스레드 동작여부
        self.count = count

    def run(self): # thread.start() --> run() 대신 실행
        while self.working:
            if self.count <= MAX:
                self.procChanged.emit(self.count) # 시그널을 내보냄
                self.count += 1 # 값 증가만 // 업무프로세스 동작하는 위치
                time.sleep(0.001) # 1ms 0.0000001 세밀하게 주면 GUI처리를 제대로 못함
            else:
                self.working = False # 멈춤

class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        uic.loadUi('./studyThread/threadApp.ui', self)
        self.setWindowTitle('쓰레드앱 v0.4')
        self.setWindowIcon(QIcon('./studyThread/thread.png'))
        self.pgbTask.setValue(0)
        # 내장 시그널
        self.btnStart.clicked.connect(self.btnStartClicked)
        # 쓰레드 생성
        self.worker = BackgroundWorker(parent=self, count=0)
        # 백그라운드 워커의 시그널을 접근 슬롯함수
        self.worker.procChanged.connect(self.procUpdated) 
        self.pgbTask.setRange(0, MAX)

    # @pyqtSlot(int) # 데코레이션
    def procUpdated(self, count):
        self.txbLog.append(f'스레드 출력 > {count}')
        self.pgbTask.setValue(count)
        print(f'스레드 출력 > {count}')

    def btnStartClicked(self):        
        self.worker.start() # 스레드 클래스 run() 실행
        self.worker.working = True
        self.worker.count = 0 # 


if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())