/**
* @file DrinkStorage.cs
* @brief ドリンクストレージクラスを定義
* @author Isao Nakamura
* @date 2020-12-10
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine
{
    /**
    * @brief ドリンクストレージクラス
    */
    public class DrinkStorage
    {
        //! ドリンクストレージスタック{ value=Drink }
        private Stack<Drink> m_storage { get; set; }

        /**
        * @brief コンストラクタ privateにしてAPPからはcreateInstanceを呼び出してもらうようにする
        */
        private DrinkStorage()
        {
            m_storage = null;
            return;
        }

        /**
        * @brief デストラクタ
        */
        ~DrinkStorage()
        {
            clear();
        }

        /**
        * @brief メンバ変数のクリア
        */
        private void clear()
        {
            if (m_storage != null)
            {
                m_storage.Clear();
                m_storage = null;
            }
        }

        /**
        * @brief ドリンクストレージが空かチェックする
        * @retval true ドリンクストレージが空
        * @retval false ドリンクストレージが空でない
        */
        public bool isEmpty()
        {
            if (m_storage != null && m_storage.Count > 0)
            {
                return false;
            }
            return true;
        }

        /**
        * @brief DrinkStorageのインスタンスを生成する
        * @param[out] drinkStorage DrinkStorageのインスタンス
        * @retval 0 正常終了
        * @retval 0以外 異常終了
        */
        public static int createInstance(out DrinkStorage drinkStorage)
        {
            int result = -1;

            // 返却値の初期化
            drinkStorage = null;

            try
            {
                drinkStorage = new DrinkStorage();
                if (drinkStorage == null)
                {
                    return result;
                }

                drinkStorage.m_storage = new Stack<Drink>();
                if (drinkStorage.m_storage == null)
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
                    drinkStorage = null;
                }
            }
            return result;
        }

        /**
        * @brief ドリンクストレージにドリンクを追加する
        * @param[in] ドリンクのインスタンス
        * @retval 0 正常終了
        * @retval 0以外 異常終了
        */
        public int pushDrink(ref Drink drink)
        {
            int result = -1;

            try
            {
                // 入力値チェック
                if (drink == null)
                {
                    return result;
                }

                if (m_storage.Count > 0)
                {
                    if (m_storage.Peek().m_name != drink.m_name)
                    {
                        // エラー
                        // 異なるドリンクが入っている
                        // Console.WriteLine("ドリンク追加エラー：既に異なる種類のドリンクが入っている。");
                        return result;
                    }
                }

                m_storage.Push(drink);

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
        * @brief ドリンクストレージからドリンクを取り出す
        * @param[out] ドリンクのインスタンス
        * @retval 0 正常終了
        * @retval 0以外 異常終了
        */
        public int popDrink(out Drink drink)
        {
            int result = -1;
            drink = null;
            try
            {
                if (m_storage.Count <= 0)
                {
                    // 売り切れ
                    result = 1;
                    return result;
                }

                drink = m_storage.Pop();
                if (drink == null)
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
                    drink = null;
                }
            }
            return result;
        }
    }
}
