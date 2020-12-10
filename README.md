# VendingMachine
オブジェクト指向課題として自動販売機を作る

## 要件
ユーザがお金を入れて、飲み物を指定すると、指定の飲み物とお釣りを出す。

## 条件
お金は、硬貨（10円、50円、100円、500円）のみとする
飲み物は、水（100円）、コーラ（150円）、お茶（130円）の3種類から1つ選ぶ
飲み物の在庫は各3本ずつとし、売り切れの場合は投入金額を返金する
釣り銭は無限にあるものとする

## インターフェイス
INPUT→飲み物の種別、投入金額
OUTPUT→飲み物の取得結果、お釣り

## 仕様
実行すると、ユーザの入力待ちになる
“飲み物の種別:投入金額”の形式で入力する
飲み物が買えた場合、「〜が買えました。お釣りは〜円です。」と出力する
飲み物が買えなかった場合、「〜が買えません。〜円足りません。」と出力する
飲み物が売り切れだった場合、「〜は売り切れです。〜円返金します。」と出力する

## 実装
VisualStudio2019 C# .NET Coreを使用して実装している。
VisualStudio2019 forMacでもビルドと実行が可能。

## 実行方法
### 1. 本GitHubリポジトリをクローンして、ディレクトリに移動する
``git clone https://github.com/IsaoNakamura/VendingMachine.git``
``cd VendingMachine``
### 2. 以下のAPPを使用するので、このAPPのディレクトリに移動する。  
[VendingMachine/App/VendingMachine](https://github.com/IsaoNakamura/VendingMachine/blob/master/App/VendingMachine)  
``cd App/VendingMachine`` 

### 3. 以下のソリューションファイルをVisualStudio2019forMacで開いてReleaseビルドする。
[VendingMachine.sln](https://github.com/IsaoNakamura/VendingMachine/blob/master/App/VendingMachine/VendingMachine.sln)  

## 実行例
### 1.実行ファイルがあるディレクトリに移動する。
``cd VendingMachine/App/VendingMachine/bin/Release/netcoreapp3.1``

### 2.実行ファイルを実行する(Macの場合)
``mono VendingMachine.exe``

## クラス図
![CLASS](https://github.com/IsaoNakamura/VendingMachine/blob/master/Doc/Images/VendingMachine_クラス図.png?raw=true)  

## シーケンス図
![SEQ](https://github.com/IsaoNakamura/VendingMachine/blob/master/Doc/Images/VendingMachine_シーケンス図.png?raw=true)  