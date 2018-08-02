﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Storage.AssetStorage;
using Rock.Web.UI;

namespace RockWeb.Blocks.Core
{
    [DisplayName( "Asset Storage System File Browser" )]
    [Category( "Core" )]
    [Description( "Manage files stored on a remote server or 3rd party cloud storage" )]
    public partial class AssetStorageSystemBrowser : RockBlock
    {
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // if needed create js here
        }

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !this.IsPostBack )
            {

                pnlModalHeader.Visible = PageParameter( "ModalMode" ).AsBoolean();
                pnlModalFooterActions.Visible = PageParameter( "ModalMode" ).AsBoolean();
                lTitle.Text = PageParameter( "Title" );

                BuildFolderTreeView();
            }

            // handle custom postback events
            string postbackArgs = Request.Params["__EVENTARGUMENT"];
            if ( !string.IsNullOrWhiteSpace( postbackArgs ) )
            {
                string[] nameValue = postbackArgs.Split( new char[] { ':' } );
                if ( nameValue.Count() == 2 )
                {
                    string eventParam = nameValue[0];
                    if ( eventParam.Equals( "folder-selected" ) )
                    {
                        BuildFolderTreeView();
                        BindFileListGrid();
                    }
                }
            }

            // ajax post if needed


        }

        private void BuildFolderTreeView()
        {
            var assetStorageService = new AssetStorageSystemService( new RockContext() );
            var sb = new StringBuilder();


            sb.AppendLine( "<ul id=\"treeview\">" );

            foreach ( var assetStorageSystem in assetStorageService.GetActiveNoTracking() )
            {
                sb.AppendFormat( "<li data-expanded='false' data-id='{0}' ><span class=''> {1}</span> \n", assetStorageSystem.Id, assetStorageSystem.Name );

                if ( hfAssetStorageId.Value.IsNullOrWhiteSpace() )
                {
                    continue;
                }
                else if (hfAssetStorageId.ValueAsInt() != assetStorageSystem.Id )
                {
                    continue;
                }

                // there is a selected storage provider and this is it, so get the folders
                assetStorageSystem.LoadAttributes();
                var component = assetStorageSystem.GetAssetStorageComponent();
                var folders = component.ListFoldersInFolder( assetStorageSystem, new Asset { Key = hfSelectedFolder.Value, Type = AssetType.Folder } );

                sb.AppendLine( "<ul>" );

                foreach ( var folder in folders )
                {
                    CreateFolderNode( assetStorageSystem, component, folder );
                }

                sb.AppendLine( "</ul>" );
            }

            sb.AppendLine( "</li>" );
            sb.AppendLine( "</ul>" );

            lblFolders.Text = sb.ToString();
            upnlFolders.Update();
        }

        private string CreateFolderNode( AssetStorageSystem assetStorageSystem, AssetStorageComponent component, Asset asset )
        {
            var sb = new StringBuilder();

            bool dataExpanded = asset.Key.Contains( hfSelectedFolder.Value );
            string selected = hfSelectedFolder.Value == asset.Key ? "selected" : string.Empty;

            sb.AppendFormat( "<li data-expanded='{0}' data-id='{1}' ><span class='{2}'> {3}</span> \n", dataExpanded, asset.Key, selected, asset.Name );


            return sb.ToString();
        }



        private void BuildFolderTreeViewOld()
        {
            var assetStorageService = new AssetStorageSystemService( new RockContext() );
            var sb = new StringBuilder();
            
            if ( hfAssetStorageId.Value.IsNotNullOrWhiteSpace() )
            {
                int i = hfAssetStorageId.ValueAsInt();
                AssetStorageSystem assetStorageSystem = assetStorageService.Get( i );
                assetStorageSystem.LoadAttributes();

                var component = assetStorageSystem.GetAssetStorageComponent();
                var folders = component.ListFolderTree( assetStorageSystem, new Asset { Key = hfSelectedFolder.Value, Type = AssetType.Folder } );

                sb.AppendLine( "<ul id=\"treeview\">" );
                foreach ( var folder in folders )
                {
                    bool dataExpanded = folder.Key.Contains( hfSelectedFolder.Value );
                    string selected = hfSelectedFolder.Value == folder.Key ? "selected" : string.Empty;
                    
                    sb.AppendFormat( "<li data-expanded='{0}' data-id='{1}' ><span class='{2}'> {3}</span> \n", dataExpanded, folder.Key, selected, folder.Name );
                }

                sb.AppendLine( "</ul>" );
            }
            else
            {
                sb.AppendLine( "<ul id=\"treeview\">" );

                foreach ( var assetStorageSystem in assetStorageService.GetActiveNoTracking() )
                {
                    sb.AppendFormat( "<li data-expanded='false' data-id='{0}' ><span class=''> {1}</span> \n", assetStorageSystem.Id, assetStorageSystem.Name );
                }

                sb.AppendLine( "</ul>" );

                lblFolders.Text = sb.ToString();
                upnlFolders.Update();
            }
        }

        protected void BindFileListGrid()
        {
            // get the selcted assetstorage ID and selected folder and display the files as assets
            var assetStorageService = new AssetStorageSystemService( new RockContext() );

            if ( hfAssetStorageId.Value.IsNotNullOrWhiteSpace() )
            {
                int i = hfAssetStorageId.ValueAsInt();
                AssetStorageSystem assetStorageSystem = assetStorageService.Get( i );
                assetStorageSystem.LoadAttributes();

                var component = assetStorageSystem.GetAssetStorageComponent();
                var files = component.ListFilesInFolder( assetStorageSystem, new Asset { Key = hfSelectedFolder.Value, Type = AssetType.Folder } );

                gFileList.DataSource = files;
                gFileList.DataBind();
            }
            
        }

        protected void lbUpload_Click( object sender, EventArgs e )
        {

        }

        protected void lbDownload_Click( object sender, EventArgs e )
        {

        }

        protected void lbRename_Click( object sender, EventArgs e )
        {

        }

        protected void lbDelete_Click( object sender, EventArgs e )
        {

        }

        protected void lbRefresh_Click( object sender, EventArgs e )
        {

        }

        protected void lbCreateFolder_Click( object sender, EventArgs e )
        {

        }

        protected void lbDeleteFolder_Click( object sender, EventArgs e )
        {

        }
    }
}