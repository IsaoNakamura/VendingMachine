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

### 4.実行ファイルがあるディレクトリに移動する。
``cd VendingMachine/App/VendingMachine/bin/Release/netcoreapp3.1``

### 5.実行ファイルを実行する(Macの場合)
``mono VendingMachine.exe``

## 実行例
#### 飲み物が買えた実行例
```
いらっしゃいませ！
これらの飲み物があります。どれになさいますか？
        お茶:   130
        コーラ: 150
        水:     100
投入している金額は0円です。
「飲み物名:投入金額」の形式で入力ください。
お茶:200
あなたが選んだ飲み物「お茶」ですね。
あなたが投入した金額は「200」円ですね。
お茶が買えました。お釣りは70円です。
```
#### 対応していない貨幣を投入した場合の実行例
```いらっしゃいませ！
これらの飲み物があります。どれになさいますか？
        お茶:   130
        コーラ: 150
        水:     100
投入している金額は0円です。
「飲み物名:投入金額」の形式で入力ください。
お茶:21
貨幣は 10円 50円 100円 500円 のみ利用可能です。
投入した金額21円をお返しします。
```

#### 投入金額が不足した場合の実行例
```
いらっしゃいませ！
これらの飲み物があります。どれになさいますか？
        お茶:   130
        コーラ: 150
        水:     100
投入している金額は0円です。
「飲み物名:投入金額」の形式で入力ください。
コーラ:120
コーラが買えません。30円足りません。
投入した金額120円をお返しします。
```

## 応用課題
### 1. 1000円が使えるように拡張する
### 2. 残金が飲み物の最低料金を超えていれば、追加　で購入できるようにする
### 3. 1/10で全額返金されるくじを実装する
通常課題のソースコードに応用課題の機能をオールインワンしております。  
通常課題状態でがんばって飲み物を全て空になったら、自動販売機を工場出荷状態にリセットし応用  課題状態に整備し直して稼働ループに入る仕様にしております。  

#### 1000円が使えるようになった実行例
```
全ての商品は売り切れです。ありがとうございました。
投入した金額0円をお返しします。


自動販売機を拡張します。
いらっしゃいませ！
これらの飲み物があります。どれになさいますか？
        お茶:   130
        コーラ: 150
        水:     100
投入している金額は0円です。
「飲み物名:投入金額」の形式で入力ください。
お茶:1001
貨幣は 10円 50円 100円 500円 1000円 のみ利用可能です。
投入した金額1001円をお返しします。
```

#### 追加購入機能が使えるようになった実行例
```
いらっしゃいませ！
これらの飲み物があります。どれになさいますか？
        お茶:   130
        コーラ: 150
        水:     100
投入している金額は0円です。
「飲み物名:投入金額」の形式で入力ください。
お茶:1000
あなたが選んだ飲み物「お茶」ですね。
あなたが投入した金額は「1000」円ですね。
お茶が買えました。残額は870円です。


いらっしゃいませ！
これらの飲み物があります。どれになさいますか？
        お茶:   130
        コーラ: 150
        水:     100
投入している金額は870円です。
「飲み物名:投入金額」の形式で入力ください。
お茶:0
あなたが選んだ飲み物「お茶」ですね。
あなたが投入した金額は「870」円ですね。
お茶が買えました。残額は740円です。
```

#### くじ機能が使えるようになった実行例
```
いらっしゃいませ！
これらの飲み物があります。どれになさいますか？
        お茶:   130
        コーラ: 150
        水:     100
投入している金額は610円です。
「飲み物名:投入金額」の形式で入力ください。
コーラ:0
あなたが選んだ飲み物「コーラ」ですね。
あなたが投入した金額は「610」円ですね。
大当たり!!
コーラを無料で差し上げます。
投入した金額610円を全てお返しします。
```

## クラス図
![CLASS](https://raw.githubusercontent.com/IsaoNakamura/VendingMachine/main/Doc/Images/VendingMachine_%E3%82%AF%E3%83%A9%E3%82%B9%E5%9B%B3.png?raw=true)  

## シーケンス図
![SEQ](https://raw.githubusercontent.com/IsaoNakamura/VendingMachine/main/Doc/Images/VendingMachine_%E3%82%B7%E3%83%BC%E3%82%B1%E3%83%B3%E3%82%B9%E5%9B%B3.png?raw=true)