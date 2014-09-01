using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//Nuget Install CoreTweet
using CoreTweet;

namespace MyTwitter
{
	/// <summary>
	/// Lisense is MIT
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			//登録してください
			//https://apps.twitter.com/
			var apiKey = "";
			var apiSecret = "";
			var accessToken = "";
			var accessTokenSecret = "";

			//
			var mt = new MyTwitter(apiKey, apiSecret, accessToken, accessTokenSecret);

			var searchWord = "お金ほしい";
			var sf = mt.SearchScreenNameList(searchWord);

			//ブレークポイントとか置いて確認してください
			if ( sf.Count != 0 )
			{
				mt.Follow(sf.First());
				mt.UnFllow(sf.First());
			}

			var tweetText = "テストなう";
			mt.Tweet(tweetText);

			var uploadImgPath = Environment.CurrentDirectory + @"\star.jpg";
			tweetText = "画像アップロードテストなう";
			mt.Tweet(tweetText, uploadImgPath);
		}
	}

	public class MyTwitter
	{
		private Tokens _tokens;

		public MyTwitter(string apiKey, string apiSecret, string accessToken, string accessTokenSecret)
		{
			//勝手に自分アカウントのアプリ連帯として登録される
			//連帯を切るとトークンの再発行が必要
			_tokens = Tokens.Create(apiKey, apiSecret, accessToken, accessTokenSecret);
		}

		public void Tweet(string text, string imagePath)
		{
			_tokens.Statuses.UpdateWithMedia(status => text, media => new FileInfo(imagePath));
		}

		public void Tweet(string text)
		{
			_tokens.Statuses.Update(status => text);
		}

		public void Follow(string targetScreenName)
		{
			_tokens.Friendships.Create(screen_name => targetScreenName);
		}

		public void UnFllow(string targetScreenName)
		{
			_tokens.Friendships.Destroy(screen_name => targetScreenName);
		}

		public List<string> SearchScreenNameList(string tweetKeyWord)
		{
			var dst = new List<string>();
			foreach ( var tweet in _tokens.Search.Tweets(q => tweetKeyWord) )
			{
				dst.Add(tweet.User.ScreenName);
			}
			return dst;
		}
	}
}