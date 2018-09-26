using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libgpgme;
using System.IO;

namespace Funcional.GnuPG
{
    public class FileManager
    {
        #region[Properties]
        //public string sKey = @"C:\OneDrive - Funcional Mais\GMUDs\EPP_2018_SPT18 - [ACESSO-L'ÓREAL] ARQUIVOS FTP\pubkey F7DEB7A8E61B6D2819A59D318314F49DDF02A067.txt";
        //public string sEmailCadastradoNaChave = "msioffi@funcionalcorp.com.br";
        public string sKey;
        public string sEmailCadastradoNaChave;

        Context ctx = new Context();
        KeyStore keyStore = null;
        IKeyStore keyring = null;
        Key[] keys = null;

        GpgmeData plain = new GpgmeMemoryData();

        public FileManager(string _sKey, string _sEmailCadastradoNaChave)
        {
            this.sKey = _sKey;
            this.sEmailCadastradoNaChave = _sEmailCadastradoNaChave;
            GpgmeFileData keyfile = new GpgmeFileData(sKey);

            keyStore = ctx.KeyStore;
            ImportResult rst = keyStore.Import(keyfile);

            keyring = ctx.KeyStore;

            keys = keyring.GetKeyList(sEmailCadastradoNaChave, false);

            keyfile.Close();
        }

        private string destination;
        private string source;

        /// <summary>
        /// Arquivo a ser encriptado
        /// </summary>
        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }

        /// <summary>
        /// Arquivo de destino após encriptar
        /// </summary>
        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                destination = value;
            }
        }
        #endregion

        public void Encrypt()
        {
            PgpKey pKey = (PgpKey)keys[0];

            UTF8Encoding utf8 = new UTF8Encoding();
            string secrettext = System.IO.File.ReadAllText(Source);
            plain.FileName = Source;
            BinaryWriter binwriter = new BinaryWriter(plain, utf8);

            binwriter.Write(secrettext.ToCharArray());
            binwriter.Flush();

            binwriter.Seek(0, SeekOrigin.Begin);

            ctx.Armor = true;
            GpgmeData cipher = new GpgmeFileData(Destination, FileMode.Create, FileAccess.ReadWrite);
            cipher.FileName = Destination;
            var result = ctx.Encrypt(new Key[] { pKey }, EncryptFlags.AlwaysTrust, plain, cipher);

            cipher.Close();
        }

        public void Decrypt()
        {
            //TBD
        }

    }
}
