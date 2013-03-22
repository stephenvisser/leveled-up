﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace LevelUp
{
    public class AddScriptFilter : Stream
    {
        private Stream _sink;

        public AddScriptFilter(Stream sink)
		{
			_sink = sink;
		}

        	public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override void Flush()
		{
			_sink.Flush();
		}

		public override long Length
		{
			get { return 0; }
		}

		private long _position;
		public override long Position
		{
			get { return _position; }
			set { _position = value; }
		}


		public override int Read(byte[] buffer, int offset, int count)
		{
			return _sink.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _sink.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_sink.SetLength(value);
		}

		public override void Close()
		{
			_sink.Close();
		}

        private string scriptToAppend =
                @"<script type='text/javascript' src='/Scripts/jquery.signalR-1.0.1.js'>
                </script>
<script type='text/javascript' src='http://localhost:9797/signalr/hubs'>
                </script>
<script type='text/javascript'>
$(function(){
$.connection.hub.url = 'http://localhost:9797/signalr' 
var chat = $.connection.levelUpHub;
  chat.client.broadcastMessage = function (message) {
        if(message=='LeveledUp')
            alert('!LeveledUp');
    };
  $.connection.hub.start().done(function () {

  });

});
</script>
</body>";

		public override void Write(byte[] buffer, int offset, int count)
		{
			byte[] data = new byte[count];
			Buffer.BlockCopy(buffer, offset, data, 0, count);
			string html = Encoding.Default.GetString(buffer);

            html = html.Replace("</body>", scriptToAppend);

			byte[] outdata = Encoding.Default.GetBytes(html);
			_sink.Write(outdata, 0, outdata.GetLength(0));
		}

    }
}