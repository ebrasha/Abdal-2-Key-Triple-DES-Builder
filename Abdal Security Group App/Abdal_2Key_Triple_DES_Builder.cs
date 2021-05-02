using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Abdal_Security_Group_App
{
    public partial class Abdal_2Key_Triple_DES_Builder : Telerik.WinControls.UI.RadForm
    {
        public Abdal_2Key_Triple_DES_Builder()
        {
            InitializeComponent();
        }

        private void EncryptToggleSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (EncryptToggleSwitch.Value == true)
            {
                DecryptToggleSwitch.Value = false;
            }
            else
            {
                DecryptToggleSwitch.Value = true;
            }
        }

        private void DecryptToggleSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (DecryptToggleSwitch.Value == true)
            {
                EncryptToggleSwitch.Value = false;
            }
            else
            {
                EncryptToggleSwitch.Value = true;
            }
        }

        private void EncDecButton_Click(object sender, EventArgs e)
        {


            try{

                if (StringTextEditor.Text != "" && SecretPasswordTextBox.Text != "")
                {

                    Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
                    // Specify 3DES for the encryption algorithm:
                    crypt.CryptAlgorithm = "3des";

                    crypt.CipherMode = "ecb";

                    // For 2-Key Triple-DES, use a key length of 128
                    // (Given that each byte's msb is a parity bit, the strength is really 112 bits).
                    crypt.KeyLength = 128;

                    // Pad with zeros
                    crypt.PaddingScheme = 3;
                    crypt.Charset = "utf-8";

                    // EncodingMode specifies the encoding of the output for
                    // encryption, and the input for decryption.
                    // It may be "hex", "url", "base64", or "quoted-printable".
                    string EncodingMode = "hex";
                    if (hexRadioButton.IsChecked)
                    {
                        EncodingMode = "hex";
                    }
                    else if (urlRadioButton.IsChecked)
                    {
                        EncodingMode = "url";
                    }
                    else if (base64RadioButton.IsChecked)
                    {
                        EncodingMode = "base64";
                    }
                    else
                    {
                        EncodingMode = "quoted-printable";
                    }

                    crypt.EncodingMode = EncodingMode;

                    // Let's create a secret key by using the MD5 hash of a password.
                    // The Digest-MD5 algorithm produces a 16-byte hash (i.e. 128 bits)
                    crypt.HashAlgorithm = "md5";
                    string keyHex = crypt.HashStringENC(SecretPasswordTextBox.Text);

                    // Set the encryption key:
                    crypt.SetEncodedKey(keyHex, "hex");
                    string encStr = "";
                    string decStr = "";

                    if (EncryptToggleSwitch.Value == true)
                    {
                        // Encrypt
                        radProgressBar1.Value2 = 0;
                        encStr = crypt.EncryptStringENC(StringTextEditor.Text);
                        ResultTextEditor.Text = encStr;
                        radProgressBar1.Value2 = 100;
                    }
                    else
                    {
                        // Now decrypt:
                        radProgressBar1.Value2 = 0;
                        decStr = crypt.DecryptStringENC(StringTextEditor.Text);
                        ResultTextEditor.Text = decStr;
                        radProgressBar1.Value2 = 100;

                    }

                }
                else
                {
                    MessageBox.Show("The String and Secret Password fields must be filled");
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

            
           

            


        }

        private void Abdal_2Key_Triple_DES_Builder_Load(object sender, EventArgs e)
        {
            // Call Global Chilkat Unlock
            Abdal_Security_Group_App.GlobalUnlockChilkat GlobalUnlock = new Abdal_Security_Group_App.GlobalUnlockChilkat();
            GlobalUnlock.unlock();
        }
    }
}
