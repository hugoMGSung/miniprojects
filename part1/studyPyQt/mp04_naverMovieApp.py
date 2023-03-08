import sys
from PyQt5 import uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from NaverApi import * 
import webbrowser # 웹브라우저 모듈

class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        uic.loadUi('./studyPyQt/naverApiMovie.ui', self)
        self.setWindowIcon(QIcon('./studyPyQt/movie.png'))

        # 검색 버튼 클릭시그널 / 슬롯함수
        self.btnSearch.clicked.connect(self.btnSearchClicked)
        # 검색어 입력후 엔터를 치면 처리
        self.txtSearch.returnPressed.connect(self.txtSearchReturned)
        self.tblResult.doubleClicked.connect(self.tblResultDoubleClicked)

    def txtSearchReturned(self):
        self.btnSearchClicked()

    def btnSearchClicked(self):
        search = self.txtSearch.text()

        if search == '':
            QMessageBox.warning(self, '경고', '영화명을 입력하세요!')
            return
        else:
            api = NaverApi() # NaverApi 클래스 객체 생성            
            node = 'movie' # movie로 변경하면 영화검색
            display = 100

            result = api.get_naver_search(node, search, 1, display)
            # print(result) 
            # 테이블위젯에 출력 기능
            items = result['items'] # json결과 중 items 아래 배열만 추출
            self.makeTable(items)

    def tblResultDoubleClicked(self):
        selected = self.tblResult.currentRow()
        url = self.tblResult.item(selected, 5).text() # url 링크컬럼 변경
        webbrowser.open(url) # 네이버 영화 웹사이트

    # 테이블 위젯에 데이터 표시 -- 네이버 영화 결과 변경
    def makeTable(self, items) -> None:
        self.tblResult.setSelectionMode(QAbstractItemView.SingleSelection) # 단일선택
        self.tblResult.setColumnCount(7) # 컬럼갯수 변경
        self.tblResult.setRowCount(len(items)) # 현재100개 행 생성
        self.tblResult.setHorizontalHeaderLabels(['영화제목', '개봉년도', '감독', '배우진', '평점', '링크', '포스터'])
        self.tblResult.setColumnWidth(0, 150)
        self.tblResult.setColumnWidth(1, 70) # 개봉년도
        self.tblResult.setColumnWidth(4, 50) # 평점        
        # 컬럼 데이터를 수정금지
        self.tblResult.setEditTriggers(QAbstractItemView.NoEditTriggers)

        for i, post in enumerate(items):  # 0, 영화
            title = self.replaceHtmlTag(post['title']) # HTML 특수문자 변환
            pubDate = post['pubDate']
            director = post['director']
            actor = post['actor']
            userRating = post['userRating']
            link = post['link']
            img_url = post['image']
            # print(image == '')
            # 230308. 포스터 이미지 추가
            if img_url != '':  # 빈값이 아니면 포스터가 있다
                data = urlopen(post['image']).read()
                image = QImage()
                image.loadFromData(data)
                imgLabel = QLabel()  
                imgLabel.setPixmap(QPixmap(image))
            # setItem(행, 열, 넣을데이터)
            self.tblResult.setItem(i, 0, QTableWidgetItem(title))
            self.tblResult.setItem(i, 1, QTableWidgetItem(pubDate))
            self.tblResult.setItem(i, 2, QTableWidgetItem(director))
            self.tblResult.setItem(i, 3, QTableWidgetItem(actor))
            self.tblResult.setItem(i, 4, QTableWidgetItem(userRating))
            self.tblResult.setItem(i, 5, QTableWidgetItem(link))
            if img_url != '':
                self.tblResult.setCellWidget(i, 6, imgLabel)

            self.tblResult.setRowHeight(i, 100) # 

    def replaceHtmlTag(self, sentence) -> str:
        result = sentence.replace('&lt;', '<') # lesser than 작다
        result = result.replace('&gt;', '>') # greater than 크다
        result = result.replace('<b>', '') # bold
        result = result.replace('</b>', '') # bold
        result = result.replace('&apos;', "'") # apostopy 홑따옴표
        result = result.replace('&quot;', '"') # quotation mark 쌍따옴표
        # 변환 안된 특수문자가 나타나면 여기 추가
        return result

if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())