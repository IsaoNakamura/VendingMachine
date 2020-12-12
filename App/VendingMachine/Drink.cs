/**
* @file Drinke.cs
* @brief ドリンククラスを定義
* @author Isao Nakamura
* @date 2020-12-10
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine
{
    /**
    * @brief ドリンククラス
    */
    public class Drink
    {
        //! 飲料名
        public string m_name { get; set; }

        /**
        * @brief コンストラクタ privateにしてAPPからはcreateInstanceを呼び出してもらうようにする
        */
        private Drink()
        {
            clear();
            return;
        }

        /**
        * @brief デストラクタ
        */
        ~Drink()
        {
            clear();
        }

        /**
        * @brief メンバ変数のクリア
        */
        private void clear()
        {
            m_name = "";
        }

        /**
        * @brief Drinkのインスタンスを生成する
        * @param[out] drink Drinkのインスタンス
        * @param[in] name 飲料名
        * @retval 0 正常終了
        * @retval 0以外 異常終了
        */
        public static int createInstance(out Drink drink, in String name)
        {
            int result = -1;

            // 返却値の初期化
            drink = null;

            try
            {
                if (name.Length <= 0)
                {
                    return result;
                }

                drink = new Drink();
                if (drink == null)
                {
                    return result;
                }

                drink.m_name = name;


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
                    drink = null;
                }
            }
            return result;
        }
    }
}
