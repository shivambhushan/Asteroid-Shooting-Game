using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace SSCSample
{

    public class TestCryptoScript : MonoBehaviour
    {

        void Start()
        {

            string password = "passwordABC";

            byte[] temp = Funcs.EncryptTextData2("ABCDEFG", password);

            print(Convert.ToBase64String(temp));

            print(Funcs.DecryptBinaryDataToText2(temp, password));

        }

    }

}
