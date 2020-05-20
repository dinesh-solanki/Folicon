﻿
Imports System.Collections.ObjectModel
Imports IGDB
Imports IGDB.Models

Namespace Modules
    Public Class Igdbf
        public shared Async Function SearchGame(query As string,client As IGDB.IGDBApi) As Task(of Game())
            Contracts.Contract.Assert(client Isnot nothing)
           Dim response= Await client.QueryAsync (Of Game)(IGDB.Client.Endpoints.Games,"search " & """"& query &"""" & "; fields name,first_release_date,total_rating,summary,cover.*;")
            Searchresultob=response 
            return response
        End Function
        public shared sub ResultPicked(result As Game)
            Contracts.Contract.requires(result Isnot nothing)
            If result.Cover Is Nothing
                throw New Exception("NoPoster")
            End If
            Dim localPosterPath = SelectedFolderPath & "\" & Fnames(FolderNameIndex) & "\" & Fnames(FolderNameIndex) &
                                  ".png"
            Dim folderPath = SelectedFolderPath & "\" & Fnames(FolderNameIndex)
            Dim folderName = Fnames(FolderNameIndex)
            Dim year=If(result.FirstReleaseDate IsNot Nothing,result.FirstReleaseDate.Value.Year,"")
            Dim posterUrl= IGDB.ImageHelper.GetImageUrl(result.Cover.Value.ImageId,ImageSize.HD720)
        AddToPickedListDataTable(localPosterPath,result.Name,"",folderPath,folderName,year)
            FolderProcessedCount += 1
            Dim tempImage As New ImageToDownload() With{
                    .LocalPath=localPosterPath,
                    .RemotePath="https://" & posterUrl.Substring(2) }
            ImgDownloadList.Add(tempImage)
        End sub
        public Shared Function ExtractGameDetailsIntoListItem(result As Game()) As ObservableCollection(Of ListItem)
            Contracts.Contract.Requires(result IsNot nothing)
            Dim items As New ObservableCollection(Of ListItem)()
            dim mediaName as String
            dim year As String
            Dim rating as String
            Dim overview as String
            For Each item In result
                mediaName = item.Name
                year = If(item.FirstReleaseDate Isnot Nothing, item.FirstReleaseDate.Value.Year, "")
                'rating = If(item.TotalRating Isnot Nothing, item.TotalRating,"") 
                overview = item.Summary
                items.Add(new ListItem(mediaName, year, "", overview))
            Next
            Return items
            End Function
    End Class
End NameSpace