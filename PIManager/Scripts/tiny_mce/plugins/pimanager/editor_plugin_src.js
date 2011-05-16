/**
 * editor_plugin_src.js
 *
 * Created for the project PI Manager
 * by J�r�my Singy
 */

(function() {
	tinymce.create('tinymce.plugins.PIManagerPlugin', {
		init : function(ed, url) {
			var t = this; //, dialect = ed.getParam('bbcode_dialect', 'punbb').toLowerCase();

			ed.onBeforeSetContent.add(function(ed, o) {
				o.content = t['_pi2html'](o.content);
			});

			ed.onPostProcess.add(function(ed, o) {
				if (o.set)
					o.content = t['_pi2html'](o.content);

				if (o.get)
					o.content = t['_html2pi'](o.content);
			});
		},

		getInfo : function() {
			return {
				longname : 'PIManager Plugin',
				author : 'J�r�my Singy',
				authorurl : 'http://www.eia-fr.ch',
				infourl : 'http://www.eia-fr.ch',
				version : tinymce.majorVersion + "." + tinymce.minorVersion
			};
		},

		// Private methods

		// HTML -> BBCode in PunBB dialect
		_html2pi : function(s) {
			s = tinymce.trim(s);

			function rep(re, str) {
				s = s.replace(re, str);
			};

			// example: <strong> to [b]
			rep(/<a.*?href=\"(.*?)\".*?>(.*?)<\/a>/gi,"$2");
			rep(/<font.*?color=\"(.*?)\".*?class=\"codeStyle\".*?>(.*?)<\/font>/gi,"$2");
			rep(/<font.*?color=\"(.*?)\".*?class=\"quoteStyle\".*?>(.*?)<\/font>/gi,"$2");
			rep(/<font.*?class=\"codeStyle\".*?color=\"(.*?)\".*?>(.*?)<\/font>/gi,"$2");
			rep(/<font.*?class=\"quoteStyle\".*?color=\"(.*?)\".*?>(.*?)<\/font>/gi,"$2");
			rep(/<span style=\"color: ?(.*?);\">(.*?)<\/span>/gi,"$2");
			rep(/<font.*?color=\"(.*?)\".*?>(.*?)<\/font>/gi,"$2");
			rep(/<span style=\"font-size:(.*?);\">(.*?)<\/span>/gi,"$2");
			rep(/<font>(.*?)<\/font>/gi,"$1");
			rep(/<img.*?src=\"(.*?)\".*?\/>/gi,"$1");
			rep(/<span class=\"codeStyle\">(.*?)<\/span>/gi,"$1");
			rep(/<span class=\"quoteStyle\">(.*?)<\/span>/gi,"$1");
			rep(/<strong class=\"codeStyle\">(.*?)<\/strong>/gi,"$1");
			rep(/<strong class=\"quoteStyle\">(.*?)<\/strong>/gi,"$1");
			rep(/<em class=\"codeStyle\">(.*?)<\/em>/gi,"$1");
			rep(/<em class=\"quoteStyle\">(.*?)<\/em>/gi,"$1");
			rep(/<u class=\"codeStyle\">(.*?)<\/u>/gi,"$1");
			rep(/<u class=\"quoteStyle\">(.*?)<\/u>/gi,"$1");
			rep(/<\/(strong|b)>/gi,"");
			rep(/<(strong|b)>/gi,"");
			rep(/<\/(em|i)>/gi,"");
			rep(/<(em|i)>/gi,"");
			rep(/<\/u>/gi,"");
			rep(/<span style=\"text-decoration: ?underline;\">(.*?)<\/span>/gi,"$1");
			rep(/<u>/gi,"[u]");
			rep(/<blockquote[^>]*>/gi,"");
			rep(/<\/blockquote>/gi,"");
			rep(/<br \/>/gi,"\n");
			rep(/<br\/>/gi,"\n");
			rep(/<br>/gi,"\n");
			rep(/<p>/gi,"<paragraph>");
			rep(/<\/p>/gi,"</paragraph>");
			rep(/&nbsp;|\u00a0/gi," ");
			rep(/&quot;/gi,"\"");
			rep(/&lt;/gi,"<");
			rep(/&gt;/gi,">");
			rep(/&amp;/gi,"&");
            
            rep(/<div>/gi, "");
            rep(/<div style=[.*?]">/gi, "");
            rep(/<\/div>/gi, "");
            
            rep(/<ul>/gi,"<list ordered=\"false\">");
            rep(/<\/ul>/gi,"</list>");
            rep(/<ol>/gi,"<list ordered=\"true\">");
            rep(/<\/ol>/gi,"</list>");
            rep(/<li>/gi,"<listItem><text>");
            rep(/<\/li>/gi,"</text></listItem>");

			return s; 
		},

		// BBCode -> HTML from PunBB dialect
		_pi2html : function(s) {
			s = tinymce.trim(s);

			function rep(re, str) {
				s = s.replace(re, str);
			};

			// example: [b] to <strong>
            rep(/<paragraph>/gi,"<p>");
			rep(/<\/paragraph>/gi,"</p>");
			rep(/\[b\]/gi,"<strong>");
			rep(/\[\/b\]/gi,"</strong>");
			rep(/\[i\]/gi,"<em>");
			rep(/\[\/i\]/gi,"</em>");
			rep(/\[u\]/gi,"<u>");
			rep(/\[\/u\]/gi,"</u>");
			rep(/\[url=([^\]]+)\](.*?)\[\/url\]/gi,"<a href=\"$1\">$2</a>");
			rep(/\[url\](.*?)\[\/url\]/gi,"<a href=\"$1\">$1</a>");
			rep(/\[img\](.*?)\[\/img\]/gi,"<img src=\"$1\" />");
			rep(/\[color=(.*?)\](.*?)\[\/color\]/gi,"<font color=\"$1\">$2</font>");
			rep(/\[code\](.*?)\[\/code\]/gi,"<span class=\"codeStyle\">$1</span>&nbsp;");
			rep(/\[quote.*?\](.*?)\[\/quote\]/gi,"<span class=\"quoteStyle\">$1</span>&nbsp;");
            
            rep(/<list ordered=\"true\">([\s\S]*?)<\/list>/gi,"<ol>$1</ol>");
            rep(/<list ordered=\"false\">([\s\S]*?)<\/list>/gi,"<ul>$1</ul>");
            rep(/<listItem>/gi,"<li>");
            rep(/<\/listItem>/gi,"</li>");
            rep(/<text>/gi,"");
            rep(/<\/text>/gi,"");

			return s; 
		}
	});

	// Register plugin
	tinymce.PluginManager.add('pimanager', tinymce.plugins.PIManagerPlugin);
})();