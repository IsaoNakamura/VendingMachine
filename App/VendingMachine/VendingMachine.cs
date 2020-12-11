/**
* @file VendingMachine.cs
* @brief 自動販売機クラスを定義
* @author Isao Nakamura
* @date 2020-12-10
*/

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace VendingMachine
{
    /**
    * @brief 自動販売機クラス
    */
    class VendingMachine
    {
        //! ドリンクストレージ辞書配列{ key=飲料名, value=ドリンクストレージ }
        private Dictionary<String, DrinkStorage> m_dictDrinkStorage { get; set; }

        //! ドリンク価格テーブル辞書配列{ key=飲料名, value=飲料価格 }
        private Dictionary<String, int> m_dictDrinkPrice { get; set; }

        //! サポートする貨幣種類リスト ex) 10 50 100 500 1000
        private List<int> m_supportMoneyType { get; set; }

        //! おみくじ機能ON/OFFフラグ
        private bool m_activeLot { get; set; }

        //! 追加購入機能ON/OFFフラグ
        private bool m_activeAdd { get; set; }

        /**
		* @brief コンストラクタ privateにしてAPPからはcreateInstanceを呼び出してもらうようにする
		*/
        private VendingMachine()
        {
            m_dictDrinkStorage = null;
            m_dictDrinkPrice = null;
            m_supportMoneyType = null;

            m_activeLot = false;
            m_activeAdd = false;
            return;
        }

        /**
		* @brief デストラクタ
		*/
        ~VendingMachine()
        {
            clear();
        }

        /**
		* @brief メンバ変数のクリア
		*/
        private void clear()
        {
            if (m_dictDrinkStorage != null)
            {
                m_dictDrinkStorage.Clear();
                m_dictDrinkStorage = null;
            }

            if (m_dictDrinkPrice != null)
            {
                m_dictDrinkPrice.Clear();
                m_dictDrinkPrice = null;
            }

            if (m_supportMoneyType != null)
            {
                m_supportMoneyType.Clear();
                m_supportMoneyType = null;
            }
        }

        /**
		* @brief VendingMachineのインスタンスを生成する
        * @param[out] machine VendingMachineのインスタンス
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        public static int createInstance(out VendingMachine machine)
        {
            int result = -1;

            // 返却値の初期化
            machine = null;

            try
            {
                machine = new VendingMachine();
                if (machine == null)
                {
                    return result;
                }

                machine.m_dictDrinkStorage = new Dictionary<String, DrinkStorage>();
                if (machine.m_dictDrinkStorage == null)
                {
                    return result;
                }

                machine.m_dictDrinkPrice = new Dictionary<String, int>();
                if (machine.m_dictDrinkPrice == null)
                {
                    return result;
                }

                machine.m_supportMoneyType = new List<int>();
                if (machine.m_supportMoneyType == null)
                {
                    return result;
                }

                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                    machine = null;
                }
            }
            return result;
        }

        /**
		* @brief 自販機を工場出荷状態に戻す。つまり、createInstanceされた直後に戻す。
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        public int reset()
        {
            int result = -1;

            try
            {
                // 前提チェック
                if (m_dictDrinkPrice == null)
                {
                    return result;
                }
                if (m_dictDrinkStorage == null)
                {
                    return result;
                }
                if (m_supportMoneyType == null)
                {
                    return result;
                }

                // 各メンバの情報を空にする
                m_supportMoneyType.Clear();
                m_dictDrinkPrice.Clear();
                m_dictDrinkStorage.Clear();
                m_activeAdd = false;
                m_activeLot = false;

                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                }
            }
            return result;
        }

        /**
		* @brief 自販機をメンテナンスする
        * 自販機のメンテナンス内容は以下である。
        * ・ドリンクストレージの追加・ドリンクの補充
        * ・各機能のON/OFF設定
        * ・製品ラインナップの設定
        * @param[in] drink_stock_num ドリンクを補充する数
        * @param[in] drink_lineup 製品ラインナップ,{ドリンク種類,価格}を要素とした配列
        * @param[in] supportMoneyType サポートする貨幣種類配列
        * @param[in] activeAdd 追加購入機能ON/OFFフラグ
        * @param[in] activeLot おみくじ機能ON/OFFフラグ
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        public int maintain
        (
            in int drink_stock_num,
            in Tuple<string, int>[] drink_lineup,
            in int[] supportMoneyType,
            in bool activeAdd,
            in bool activeLot
        )
        {
            int result = -1;


            try
            {
                if (reset() != 0)
                {
                    return result;
                }

                foreach (int moneyType in supportMoneyType)
                {
                    m_supportMoneyType.Add(moneyType);
                }
                // 昇順に整列
                m_supportMoneyType.Sort();

                foreach (Tuple<string, int> element in drink_lineup)
                {
                    // 設定するドリンク価格がサポートしている貨幣とマッチしているかチェック
                    // 例えば、水が101円で設定しており最低貨幣が10円の場合お釣りが出ないトラブルがおきる
                    bool isPriceOK = false;
                    if (checkInputMoneyType(out isPriceOK, element.Item2) != 0)
                    {
                        return result;
                    }
                    if (!isPriceOK)
                    {
                        return result;
                    }

                    // 価格テーブルにドリンク名と金額を追加
                    m_dictDrinkPrice[element.Item1] = element.Item2;

                    // ドリンクストレージをメンテナンスのために取り出し
                    DrinkStorage drinkStorage = null;
                    if (maintainStorage(out drinkStorage, element.Item1) != 0)
                    {
                        return result;
                    }
                    if (drinkStorage == null)
                    {
                        return result;
                    }

                    //ドリンクをストレージに追加
                    for (int i = 0; i < drink_stock_num; i++)
                    {
                        Drink drink = null;
                        if (Drink.createInstance(out drink, element.Item1) != 0)
                        {
                            return result;
                        }

                        if (drinkStorage.pushDrink(ref drink) != 0)
                        {
                            return result;
                        }
                    }
                }

                // 追加購入機能を設定する
                m_activeAdd = activeAdd;
                
                // おみくじ機能を設定する
                m_activeLot = activeLot;

                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                    reset();
                }
            }
            return result;
        }

        /**
		* @brief ドリンクストレージをメンテナンスする
        * ドリンクストレージにドリンクを入れる場合に使用する
        * ドリンク種類に応じたドリンクストレージを取得および生成をする
        * @param[out] drinkStorage ドリンク保持するストレージ
        * @param[in] drinkName ドリンク種類名
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        private int maintainStorage(out DrinkStorage drinkStorage, in String drinkName)
        {
            int result = -1;

            // 返却値の初期化
            drinkStorage = null;

            try
            {
                if (m_dictDrinkStorage.ContainsKey(drinkName))
                {
                    // 存在する
                    drinkStorage = m_dictDrinkStorage[drinkName];
                }
                else
                {
                    // 存在しなければこの時点で生成
                    if (DrinkStorage.createInstance(out drinkStorage) != 0)
                    {
                        return result;
                    }
                    m_dictDrinkStorage.Add(drinkName, drinkStorage);
                }

                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                    drinkStorage = null;
                }
            }
            return result;
        }

        /**
		* @brief ドリンクストレージを取得する
        * ドリンクストレージからドリンクを取り出す場合に使用する
        * ドリンク種類に応じたドリンクストレージを取得する
        * @param[out] drinkStorage ドリンク保持するストレージ
        * @param[in] drinkName ドリンク種類名
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        private int getStorage(out DrinkStorage drinkStorage, in String drinkName)
        {
            int result = -1;

            // 返却値の初期化
            drinkStorage = null;

            try
            {
                if (!m_dictDrinkStorage.ContainsKey(drinkName))
                {
                    // 存在しない
                    return result;

                }

                // 存在する
                drinkStorage = m_dictDrinkStorage[drinkName];


                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                    drinkStorage = null;
                }
            }
            return result;
        }

        /**
		* @brief 全てのドリンクストレージが空かチェックする
		* @retval true 全てのドリンクストレージが空
		* @retval false ドリンクストレージのどれか1つでも空でない
		*/
        private bool isEmptyAllStorage()
        {
            bool result = false;
            try
            {
                bool isALlEmpty = true;
                foreach (KeyValuePair<string, DrinkStorage> element in m_dictDrinkStorage)
                {
                    if(!element.Value.isEmpty())
                    {
                        isALlEmpty = false;
                        break;
                    }
                }

                result = isALlEmpty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
            }
            return result;
        }

        /**
		* @brief 投入金額の貨幣種類をチェックする
        * ユーザには「飲み物名:投入金額」の形式で入力させており
        * どのような組み合わせで金額を投入しているかはチェックできないので
        * 一番小さい貨幣より細かい投入金額になってないかのみチェックする
        * @param[out] isOK チェック結果 trueならOK falseならNG
        * @param[in] input_amount 投入金額
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        private int checkInputMoneyType(out bool isOK, in int input_amount)
        {
            int result = -1;
            isOK = false;
            try
            {
                if(m_supportMoneyType==null)
                {
                    return result;
                }

                if(m_supportMoneyType.Count <= 0)
                {
                    return result;
                }

                // ソート済みである前提
                int minType = m_supportMoneyType.First();

                if (input_amount % minType == 0)
                {
                    isOK = true;
                }
                else
                {
                    // エラーメッセージ
                    string moneyTypesStr = "";
                    foreach (int moneyType in m_supportMoneyType)
                    {
                        moneyTypesStr = moneyTypesStr + moneyType + "円 ";
                    }
                    Console.WriteLine("貨幣は {0}のみ利用可能です。", moneyTypesStr);
                }

                // ここまでくれば正常終了
                result = 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                    isOK = false;
                }
            }
            return result;
        }

        /**
		* @brief 投入した金額を返却してリセットする
        * @param[out] isOK チェック結果 trueならOK falseならNG
        * @param[in] input_amount 投入金額
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        private int resetAmount(ref int input_amount)
        {
            int result = -1;

            try
            {
                Console.WriteLine("投入した金額{0}円をお返しします。", input_amount);
                Console.WriteLine("\n");
                input_amount = 0;

                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                }
            }
            return result;
        }

        /**
		* @brief 取引処理
        * 投入金額からドリンク代を徴収してドリンクとお釣りをユーザに渡す
        * おみくじ機能がONの場合、くじを引き当たりならドリンクを無料にする
        * 追加購入機能がONの場合、ドリンク代徴収後の投入金額が商品の中で一番安い値段以上だったら追加購入可能とする
        * @param[in,out] input_amount 投入金額
        * @param[in,out] drink ドリンク
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        private int transaction(ref int input_amount, ref Drink drink)
        {
            int result = -1;

            try
            {
                int drink_price = m_dictDrinkPrice[drink.m_name];


                bool isBingo = false;
                if (m_activeLot)
                {
                    // おみくじ機能がONの場合
                    if (drawLot(out isBingo) != 0)
                    {
                        return result;
                    }
                }

                if (isBingo)
                {
                    Console.WriteLine("大当たり!!");

                    Console.WriteLine("{0}を無料で差し上げます。", drink.m_name);
                    Console.WriteLine("投入した金額{0}円を全てお返しします。", input_amount);
                    Console.WriteLine("\n");

                    // お釣りを利用者に渡したとして変数の値をリセットする
                    input_amount = 0;
                }
                else
                {
                    // 投入金額からドリンク代を徴収
                    input_amount = input_amount - drink_price;

                    bool isAdd = false;
                    if (m_activeAdd)
                    {
                        //追加購入機能がONの場合

                        // 商品の中で一番安い値段をピックアップ
                        List<int> prices = m_dictDrinkPrice.Values.ToList();
                        prices.Sort();
                        int min_price = prices.First();

                        if (input_amount >= min_price)
                        {
                            isAdd = true;
                        }
                    }

                    if (isAdd)
                    {
                        Console.WriteLine("{0}が買えました。残額は{1}円です。", drink.m_name, input_amount);
                        Console.WriteLine("\n");
                    }
                    else
                    {
                        Console.WriteLine("{0}が買えました。お釣りは{1}円です。", drink.m_name, input_amount);
                        Console.WriteLine("\n");

                        // お釣りを利用者に渡したとして変数の値をリセットする
                        input_amount = 0;
                    }
                }
                // ドリンクを利用者に渡したとして変数の値をリセットする
                drink = null;

                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                }
            }
            return result;
        }

        /**
		* @brief 稼働準備
        * 稼働準備ができていなければ異常終了値を返す
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        private int ready()
        {
            int result = -1;

            try
            {
                // 前提チェック
                if (m_dictDrinkPrice == null)
                {
                    return result;
                }
                if (m_dictDrinkStorage == null)
                {
                    return result;
                }
                if (m_supportMoneyType == null)
                {
                    return result;
                }

                // ストレージが整備済みかチェック
                if (isEmptyAllStorage())
                {
                    return result;
                }

                // 対応する貨幣が整備済みかチェック
                if (m_supportMoneyType.Count <= 0)
                {
                    return result;
                }

                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                }
            }
            return result;
        }

        /**
		* @brief くじを引く
        * @param[out] isBingo 大当たり有無フラグ
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        private int drawLot(out bool isBingo)
        {
            int result = -1;
            isBingo = false;

            try
            {
                // ランダムモジュールを準備
                Random random = new Random();
                if (random == null)
                {
                    return result;
                }

                // 0から9の番号をランダムに取得
                int draw_number = random.Next(0, 10);
                if (draw_number == 0)
                {
                    // 0を当り番号とする
                    isBingo = true;
                }

                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                    isBingo = false;
                }
            }
            return result;
        }

        /**
		* @brief 自動販売機のメインループ処理
        * ユーザと対話で飲料の販売やり取りを行う
        * 実行する前提として、maintainメソッドで自動販売機の整備が終わっていないといけない
        * 全ての商品が売り切れたらループ処理を抜ける
		* @retval 0 正常終了
		* @retval 0以外 異常終了
        */
        public int mainLoop()
        {
            int result = -1;
            int input_amount = 0;

            try
            {
                // 稼働準備
                if (ready() != 0)
                {
                    return result;
                }

                // 商品ラインナップを用意
                List<String> lineup = m_dictDrinkPrice.Keys.ToList();
                lineup.Sort();

                while (true)
                {
                    Console.WriteLine("いらっしゃいませ！");
                    Console.WriteLine("これらの飲み物があります。どれになさいますか？");
                    foreach (String drink_name in lineup)
                    {
                        Console.WriteLine("\t{0}:\t{1}", drink_name, m_dictDrinkPrice[drink_name]);
                    }
                    Console.WriteLine("投入している金額は{0}円です。", input_amount);
                    Console.WriteLine("「飲み物名:投入金額」の形式で入力ください。");
                    if (input_amount > 0)
                    {
                        Console.WriteLine("投入した金額を返却する場合は「q」を入力してください。");
                    }

                    // 入力形式チェック
                    string input_line = Console.ReadLine();
                    string[] inputs = input_line.Split(':');
                    if (inputs.Length != 2)
                    {
                        if (inputs.Length == 1 &&input_amount > 0 && inputs[0].Equals("q"))
                        {
                            resetAmount(ref input_amount);
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("正しく「飲み物名:投入金額」の形式で入力ください。");
                            Console.WriteLine("\n");
                            continue;
                        }
                    }
                    // 入力値解析
                    String　select_name = inputs[0];
                    input_amount = input_amount + int.Parse(inputs[1]);

                    // 投入貨幣チェック
                    bool isMoneyTypeOK = false;
                    if (checkInputMoneyType(out isMoneyTypeOK, in input_amount) != 0)
                    {
                        return result;
                    }
                    if(!isMoneyTypeOK)
                    {
                        resetAmount(ref input_amount);
                        continue;
                    }

                    // 飲み物名チェック
                    if (!m_dictDrinkPrice.ContainsKey(select_name))
                    {
                        Console.WriteLine("「{0}」はありません。", select_name);
                        resetAmount(ref input_amount);
                        continue;
                    }

                    int drink_price = m_dictDrinkPrice[select_name];

                    // 投入額チェック
                    if (input_amount < drink_price)
                    {
                        Console.WriteLine("{0}が買えません。{1}円足りません。", select_name, drink_price-input_amount);
                        resetAmount(ref input_amount);
                        continue;
                    }


                    Console.WriteLine("あなたが選んだ飲み物「{0}」ですね。", select_name);
                    Console.WriteLine("あなたが投入した金額は「{0}」円ですね。", input_amount);

                    // ドリンクストレージにアクセス
                    DrinkStorage drinkStorage = null;
                    if (getStorage(out drinkStorage, select_name) != 0)
                    {
                        return result;
                    }

                    // 在庫チェック
                    if (drinkStorage.isEmpty())
                    {
                        Console.WriteLine("{0}は売り切れです。", select_name);
                        resetAmount(ref input_amount);
                        continue;
                    }

                    // ストレージからドリンクを出す
                    Drink drink = null;
                    if (drinkStorage.popDrink(out drink) != 0)
                    {
                        return result;
                    }

                    // 取引処理
                    if (transaction(ref input_amount, ref drink) != 0)
                    {
                        return result;
                    }

                    // ループ完了チェック
                    if (isEmptyAllStorage())
                    {
                        Console.WriteLine("全ての商品は売り切れです。ありがとうございました。");
                        resetAmount(ref input_amount);
                        break;
                    }
                }

                // ここまでくれば正常終了
                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (result != 0)
                {
                    Console.WriteLine("致命的なエラーが発生しました。");
                    resetAmount(ref input_amount);
                }
            }
            return result;
        }

    }
}
