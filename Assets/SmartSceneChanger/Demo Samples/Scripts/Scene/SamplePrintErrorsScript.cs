using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;

namespace SSCSample
{

    public class SamplePrintErrorsScript : MonoBehaviour
    {

        public void printCurrentErrors()
        {

            if(DialogManager.Instance.errorDialogMessagesStack.Count <= 0)
            {
                print("No errors in errorDialogMessagesStack");
                return;
            }

            // ----------------------------

            DialogMessages temp = null;

            foreach (var val in DialogManager.Instance.errorDialogMessagesStack)
            {

                if(val is DialogMessages)
                {

                    temp = (DialogMessages)val;

                    print(temp.mainMessage);

                }

            }

        }

    }

}
