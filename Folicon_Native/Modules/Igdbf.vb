﻿
Imports System.Collections.ObjectModel
Imports IGDB
Imports IGDB.Models

Namespace Modules
    Module Igdbf
        Public Async Function SearchGame(query As String, client As IGDBApi) As Task(Of Game())
            Contracts.Contract.Assert(client IsNot Nothing)
            Dim response = Await client.QueryAsync(Of Game)(IGDB.Client.Endpoints.Games, "search " & """" & query & """" & "; fields name,first_release_date,total_rating,summary,cover.*;")
            Searchresultob = response
            Return response
        End Function
        Public Sub ResultPicked(result As Game)
            Contracts.Contract.Requires(result IsNot Nothing)
            If result.Cover Is Nothing Then
                Throw New Exception("NoPoster")
            End If
            Dim localPosterPath = SelectedFolderPath & "\" & Fnames(FolderNameIndex) & "\" & Fnames(FolderNameIndex) &
                                  ".png"
            Dim folderPath = SelectedFolderPath & "\" & Fnames(FolderNameIndex)
            Dim folderName = Fnames(FolderNameIndex)
            Dim year = If(result.FirstReleaseDate IsNot Nothing, result.FirstReleaseDate.Value.Year, "")
            Dim posterUrl = ImageHelper.GetImageUrl(result.Cover.Value.ImageId, ImageSize.HD720)
            AddToPickedListDataTable(localPosterPath, result.Name, "", folderPath, folderName, year)
            FolderProcessedCount += 1
            Dim tempImage As New ImageToDownload() With {
                    .LocalPath = localPosterPath,
                    .RemotePath = "https://" & posterUrl.Substring(2)}
            ImgDownloadList.Add(tempImage)
        End Sub
        Public Function ExtractGameDetailsIntoListItem(result As Game()) As ObservableCollection(Of ListItem)
            Contracts.Contract.Requires(result IsNot Nothing)
            Dim items As New ObservableCollection(Of ListItem)()
            Dim mediaName As String
            Dim year As String
            Dim overview As String
            Dim poster As String
            For Each item In result
                mediaName = item.Name
                year = If(item.FirstReleaseDate IsNot Nothing, item.FirstReleaseDate.Value.Year, "")
                overview = item.Summary
                poster = If(item.Cover IsNot Nothing, "https://" & ImageHelper.GetImageUrl(item.Cover.Value.ImageId, ImageSize.HD720).Substring(2), Nothing)
                items.Add(New ListItem(mediaName, year, "", overview, poster))
            Next
            Return items
        End Function
    End Module
End NameSpace