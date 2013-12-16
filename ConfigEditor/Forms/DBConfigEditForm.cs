/**
 * 文件名：DBConfigEditForm.cs
 * 说明：DB块添加或编辑窗体类
 * 作者：刘风彬
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 刘风彬 	2013-12-15		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;
using ConfigEditor.Util;
using ConfigEditor.Core.Models;
using ConfigEditor.Core.ViewModels;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// DB块添加或编辑窗体类
    /// </summary>
    public partial class DBConfigEditForm : Form
    { 
        //日志
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //主窗体
        private MainForm _parentForm;

        //用户操作
        private UserActions _action;

        //视图模型
        private DBConfigViewModel _model;

        /// <summary>
        /// 实体模型
        /// </summary>
        public DBConfigViewModel Model { get { return _model; } }

        public DBConfigEditForm()
        {
            InitializeComponent();
        }

         /// <summary>
        /// 添加DB块
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="action"></param>
        public DBConfigEditForm(MainForm parentForm)
            : this()
        {
            this.Text = "添加DB块";
            this._parentForm = parentForm;
            this._action = UserActions.Add;
        }

        
        /// <summary>
        /// 编辑DB块
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="model"></param>
        public DBConfigEditForm(MainForm parentForm, DBConfigViewModel model)
            : this()
        {
            this.Text = "编辑DB块";
            this._parentForm = parentForm;
            this._action = UserActions.Edit;
            this._model = model;
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void DBConfigEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._action == UserActions.Add)
                {
                    this.txtConnection.Text = "S7:[S7 connection_1]";
                    this.txtDB.Text = "DB1";
                    this.txtType.Text = "WORD";
                    //this.txtStaAddress.Text = "2";
                    this.txtLength.Text = "1";
                    this.cmbAccess.SelectedIndex = 0;
                   
                }
                else if (this._action == UserActions.Edit)
                {
                    
                    this.txtConnection.Text = this._model.Connection;
                    this.txtDB.Text = this._model.DB;
                    this.txtType.Text = this._model.DBType;
                    this.txtCode.Text = this._model.Code.ToString();
                    this.txtStaAddress.Text = this._model.StartAddress;
                    this.chkIsEnable.Checked = this._model.IsEnable;
                    this.cmbAccess.SelectedIndex = (int)this._model.Accessibility;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckUserInputs())
                {
                    return;
                }

                if (this._action == UserActions.Add)
                {
                    this._model = new DBConfigViewModel();
                    this._model.DB = this.txtDB.Text;
                    this._model.Accessibility = (AccessRights)this.cmbAccess.SelectedIndex;
                    this._model.Code = string.IsNullOrEmpty(this.txtCode.Text) ? null : (int?)Convert.ToInt32(this.txtCode.Text);
                    this._model.DBType= this.txtType.Text;
                    this._model.StartAddress = this.txtStaAddress.Text;
                    this._model.Length = this.txtLength.Text;
                    this._model.Connection = this.txtConnection.Text;
                    this._model.IsEnable = this.chkIsEnable.Checked;
                }

                else
                {
                    //this._model = new DBConfigViewModel();
                    this._model.DB = this.txtDB.Text;
                    this._model.Accessibility = (AccessRights)this.cmbAccess.SelectedIndex;
                    this._model.Code = string.IsNullOrEmpty(this.txtCode.Text) ? null : (int?)Convert.ToInt32(this.txtCode.Text);
                    this._model.DBType = this.txtType.Text;
                    this._model.StartAddress = this.txtStaAddress.Text;
                    this._model.Length = this.txtLength.Text;
                    this._model.Connection = this.txtConnection.Text;
                    this._model.IsEnable = this.chkIsEnable.Checked;
 
                }
                
                this.DialogResult = DialogResult.OK;


            }

            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

              /// <summary>
        /// 获取最新名称
        /// </summary>
        /// <returns></returns>
        //private string GetNewItemName()
        //{
        //    var query = from item in this._model.DB
        //                where item.name.StartsWith("Item") || item.DB.StartsWith("item")
        //                select item;

        //    if (query.Count() == 0)
        //    {
        //        return "Item1";
        //    }

        //    int n;
        //    int xx = query.Where(obj => obj.DB.Length > 4 && Int32.TryParse(obj.DB.Substring(4), out n)).Select(obj => Convert.ToInt32(obj.Name.Substring(4))).Max();

        //    return "Item" + (++xx).ToString();
        //}


         /// <summary>
        /// 校验用户输入
        /// </summary>
        /// <returns></returns>
        private bool CheckUserInputs()
        {
            if (string.IsNullOrEmpty(this.txtConnection.Text))
            {
                MessageBox.Show("连接名称不能为空且必须为S7:[S7 connection_1]格式。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(this.txtDB.Text))
            {
                MessageBox.Show("名称不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(this.txtType.Text))
            {
                MessageBox.Show("数据类型不能为空且必须为‘WORD’或‘B’格式。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }


            if (string.IsNullOrEmpty(this.txtStaAddress.Text))
            {
                MessageBox.Show("起始地址不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!Regex.IsMatch(this.txtStaAddress.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("起始地址长度必须为正整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!string.IsNullOrEmpty(this.txtCode.Text) && !Regex.IsMatch(this.txtCode.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("识别码必须为整数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }
         
        

        private void btnCancle_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      
    }
}
