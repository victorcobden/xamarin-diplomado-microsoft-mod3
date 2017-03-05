﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace Contacts
{
	public class AzureClient
	{
		private IMobileServiceClient _client;
		private IMobileServiceSyncTable<Contact> _table;
		const string dbPath = "contactDb";
		private const string serviceUri = "http://victorcontacts.azurewebsites.net";

		public AzureClient()
		{
			createAzureClient();
		}

		void createAzureClient()
		{
			if (string.IsNullOrWhiteSpace(serviceUri))
				throw new Exception("Debes introducir la url de tu servicio Azure");
			_client = new MobileServiceClient(serviceUri);
			var store = new MobileServiceSQLiteStore(dbPath);
			store.DefineTable<Contact>();
			_client.SyncContext.InitializeAsync(store);
			_table = _client.GetSyncTable<Contact>();
		}

		public async Task<IEnumerable<Contact>> GetContacts()
		{
			var empty = new Contact[0];
			try
			{
				if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
					await SyncAsync();

				return await _table.ToEnumerableAsync();
			}
			catch (Exception ex)
			{
				return empty;
			}
		}

		public async void AddContact(Contact contact)
		{
			await _table.InsertAsync(contact);

		}

		public async Task SyncAsync()
		{
			ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
			try
			{
				await _client.SyncContext.PushAsync();

				await _table.PullAsync("allContacts", _table.CreateQuery());
			}
			catch (MobileServicePushFailedException pushEx)
			{
				if (pushEx.PushResult != null)
					syncErrors = pushEx.PushResult.Errors;
			}
		}


		public async Task CleanData()
		{
			await _table.PurgeAsync("allContacts", _table.CreateQuery(), new System.Threading.CancellationToken());
		}
	}
}
