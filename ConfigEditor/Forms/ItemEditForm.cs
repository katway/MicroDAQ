/**
 * 文件名：SerialPortEditForm.cs
 * 说明：变量添加或编辑窗体类
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-29		创建文件
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
using ConfigEditor.Core.ViewModels;
using ConfigEditor.Core.Models;

namespace ConfigEditor.Forms
{
    /// <summary>
    /// 变量添加或编辑窗体类
    /// </summary>
    public partial class ItemEditForm : Form
    {
        //日志
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //主窗体
        private MainForm _parentForm;

        //用户操作
        private UserActions _action;

        //视图模型
        private ItemViewModel _model;

        /// <summary>
        /// 实体模型
        /// </summary>
        public ItemViewModel Model { get { return _model; } }

        public ItemEditForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加变量
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="action"></param>
        public ItemEditForm(MainForm parentForm)
            : this()
        {
            this.Text = "添加变量";
            this._parentForm = parentForm;
            this._action = UserActions.Add;
        }

        /// <summary>
        /// 编辑变量
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="model"></param>
        public ItemEditForm(MainForm parentForm, ItemViewModel model)
            : this()
        {
            this.Text = "编辑变量";
            this._parentForm = parentForm;
            this._action = UserActions.Edit;
            this._model = model;
        }
    }
}
