# csharp-toyproject-2024

**스케줄관리 프로그램** 

## 배경
개인 일정을 캘린더에 저장하여 효율적으로 관리하고 조직할 수 있는 프로그램 설계

## 기능
    - 로그인한 후 날짜를 지정해 일정을 입력하고 저장 및 삭제 
    - 저장한 일정에 대한 실행 여부를 체크리스트로 표시 가능
    - 해당 일정에 관련된 메모 작성 가능

## 특징
- DateTimePicker와 MonthCalendar를 연동시켜 날짜 지정시 텍스트박스에 표시 및 DB에 저장
- 날짜가 범위인 경우에도 일정 설정 가능

![실행화면](https://raw.githubusercontent.com/LEUNSU/csharp-toyproject-2024/main/images/cs004.png)

![실행화면](https://raw.githubusercontent.com/LEUNSU/csharp-toyproject-2024/main/images/cs009.png)

- 로그인한 계정의 정보 DB와 연동하여 저장 및 삭제

![DB화면](https://raw.githubusercontent.com/LEUNSU/csharp-toyproject-2024/main/images/cs011.png)

    - 로그인 계정 테이블

![DB화면](https://raw.githubusercontent.com/LEUNSU/csharp-toyproject-2024/main/images/cs012.png)

    - 메모 내용 테이블

![DB화면](https://raw.githubusercontent.com/LEUNSU/csharp-toyproject-2024/main/images/cs013.png)

    - 일정 테이블 

## 프로그램 실행
- 로그인 화면

![로그인](https://raw.githubusercontent.com/LEUNSU/csharp-toyproject-2024/main/images/cs003.png)

- 회원가입 화면

![회원가입](https://raw.githubusercontent.com/LEUNSU/csharp-toyproject-2024/main/images/cs018.png)

- 날짜 지정한 후 일정 입력, 저장 및 삭제

![일정저장](https://raw.githubusercontent.com/LEUNSU/csharp-toyproject-2024/main/images/cs006.png)

- 저장한 일정 선택 후 메모 작성 및 저장. 일정은 수정 가능

![메모저장](https://raw.githubusercontent.com/LEUNSU/csharp-toyproject-2024/main/images/cs007.png)