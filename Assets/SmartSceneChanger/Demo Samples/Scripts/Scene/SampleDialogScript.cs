using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;

namespace SSCSample
{

    public class SampleDialogScript : MonoBehaviour
    {

        DialogMessages m_message = new DialogMessages();

        public void showYesNoDialog()
        {

            this.m_message.clear();

            this.m_message.category = DialogMessages.MessageCategory.Confirmation;
            this.m_message.title = "Title";
            this.m_message.mainMessage = "YesNo mainMessage";
            this.m_message.subMessage = "subMessage";
            this.m_message.anyMessage = "anyMessage";

            DialogManager.Instance.showYesNoDialog(this.m_message, this.yesCallback, this.noCallback);

        }

        public void showOkDialog()
        {

            this.m_message.clear();

            this.m_message.category = DialogMessages.MessageCategory.Confirmation;
            this.m_message.title = "Title";
            this.m_message.mainMessage = "OK mainMessage";
            this.m_message.subMessage = "subMessage";
            this.m_message.anyMessage = "anyMessage";

            DialogManager.Instance.showOkDialog(this.m_message, this.okCallback);

        }

        public void showProgressDialog()
        {

            this.m_message.clear();

            DialogManager.Instance.consecutiveShowing = true;

            this.m_message.category = DialogMessages.MessageCategory.Confirmation;
            this.m_message.title = "Title";
            this.m_message.mainMessage = "Show Progress?";
            this.m_message.subMessage = "subMessage";
            this.m_message.anyMessage = "anyMessage";

            DialogManager.Instance.showYesNoDialog(this.m_message, () =>
            {

                this.m_message.clear();

                this.m_message.category = DialogMessages.MessageCategory.Confirmation;
                this.m_message.title = "Title";
                this.m_message.mainMessage = "Progress mainMessage";
                this.m_message.subMessage = "subMessage";
                this.m_message.anyMessage = "anyMessage";

                DialogManager.Instance.showProgressDialog(this.m_message, null);

                StartCoroutine(this.progress());

            },
            () =>
            {
                DialogManager.Instance.finishDialog(null);
            });

        }

        IEnumerator progress()
        {

            float time = 0.0f;
            float targetSeconds = 3.0f;

            while(time <= targetSeconds)
            {

                time += Time.deltaTime;

                yield return null;

                DialogManager.Instance.setProgress(time / targetSeconds);

            }

            // finish
            {
                DialogManager.Instance.setProgress(1.0f);
                yield return null;
            }

            // finishProgressDialog
            {
                DialogManager.Instance.finishProgressDialog(() =>
                {

                    this.m_message.category = DialogMessages.MessageCategory.Confirmation;
                    this.m_message.title = "Title";
                    this.m_message.mainMessage = "OK mainMessage";
                    this.m_message.subMessage = "subMessage";
                    this.m_message.anyMessage = "anyMessage";

                    DialogManager.Instance.showOkDialog(this.m_message, () =>
                    {
                        DialogManager.Instance.consecutiveShowing = false;
                        DialogManager.Instance.finishDialog(null);
                    });

                    
                });
            }

        }

        void yesCallback()
        {
            print("yesCallback");
        }

        void noCallback()
        {
            print("noCallback");
        }

        void okCallback()
        {
            print("okCallback");
        }

    }

}
