function bl_google_cookie_mapping() {
	var imgElem = document.createElement("img");
	var divElem = document.createElement("div");
	divElem.appendChild(imgElem);

	imgElem.width = 0;
	imgElem.height = 0;
	imgElem.src = google_cookie_mapping_url();
	imgElem.alt = "";
	imgElem.setAttribute('style', 'width: 0px; height: 0px;');

	divElem.setAttribute('style', 'position:absolute; left:0px; top: 0px; visibility: hidden;');

	var body = document.getElementsByTagName('body')[0];
	body.insertBefore(divElem, null);
}

function google_cookie_mapping_url() {
	var gcm_url="http://cm.g.doubleclick.net/pixel?google_nid=miad_apac&google_ula=4520050&google_cm&google_sc";
	if(document.location.protocol=="https:"){
		gcm_url="https://cm.g.doubleclick.net/pixel?google_nid=miad_apac&google_ula=4520050&google_cm&google_sc";
	}
	return gcm_url;
}


function ambient_cookie_mapping_url() {
	var amcm_url="http://s-cm.send.adnetwork.vn/247/cm?pid=1368173075";
	if(document.location.protocol=="https:"){
		amcm_url="https://s-cm.send.adnetwork.vn/247/cm?pid=1368173075";
	}
	return amcm_url;
}

function openx_cookie_mapping_url() {
	var openxcm_url="http://jp-u.openx.net/w/1.0/cm?id=55c29392-bd57-475a-abdf-749701255a32&r=http%3A%2F%2Fcm.send.microadinc.com%2Fox%2Fcm%3Fid%3D";
	if(document.location.protocol=="https:"){
		openxcm_url="https://jp-u.openx.net/w/1.0/cm?id=55c29392-bd57-475a-abdf-749701255a32&r=https%3A%2F%2Fcm.send.microadinc.com%2Fox%2Fcm%3Fid%3D";
	}
	return openxcm_url;
}

function korea_google_cookie_mapping_url() {
	var kgcm_url="http://cm.g.doubleclick.net/pixel?google_nid=miad_korea&google_ula=6587825&google_cm";
	if(document.location.protocol=="https:"){
		kgcm_url="https://cm.g.doubleclick.net/pixel?google_nid=miad_korea&google_ula=6587825&google_cm";
	}
	return kgcm_url;
}

function  korea_openx_cookie_mapping_url() {
	var openxcm_url="http://jp-u.openx.net/w/1.0/cm?id=80972240-2f97-41c3-9156-a4b3edb41825&r=http%3A%2F%2Fcm.send.microadinc.com%2Fox%2Fcm%3Fid%3D";
	if(document.location.protocol=="https:"){
		openxcm_url="https://jp-u.openx.net/w/1.0/cm?id=80972240-2f97-41c3-9156-a4b3edb41825&r=https%3A%2F%2Fcm.send.microadinc.com%2Fox%2Fcm%3Fid%3D";
	}
	return openxcm_url;
}

function bl_cookie_mapping_img(url) {
	if (url === undefined || url === null || url == "" ) {
		return;
	}

	var imgElem = document.createElement("img");
	var divElem = document.createElement("div");
	divElem.appendChild(imgElem);

	imgElem.width = 0;
	imgElem.height = 0;
	imgElem.src = url;
	imgElem.alt = "";
	imgElem.setAttribute('style', 'width: 0px; height: 0px;');

	divElem.setAttribute('style', 'position:absolute; left:0px; top: 0px; visibility: hidden;');

	var body = document.getElementsByTagName('body')[0];
	body.insertBefore(divElem, null);
}



(function(){

	function track_host() {
		var track_host="http://d-track.send.microadinc.com";
		if(document.location.protocol=="https:"){
			track_host="https://d-track.send.microadinc.com";
		}
		return track_host;
	}

	function pc_track(co_account_id, group_id, country_id, version) {
		var encode_url=escape(document.referrer);
		var blade_query="co_account_id="+co_account_id+"&group="+group_id+"&country_id="+country_id+"&ver="+version+"&referrer="+encode_url;
		var blade_url=track_host()+"/bl_track_with_gcm.cgi?"+blade_query;
		var blade_script = null;
		var addScript = function(){
			blade_script = document.createElement('script');
			blade_script.src = blade_url;
			blade_script.type = "text/javascript";
			var s = document.getElementsByTagName('script')[0];
			s.parentNode.insertBefore(blade_script, s);
		};

		if (window.addEventListener) {
			document.readyState == 'loading' ? document.addEventListener('DOMContentLoaded', addScript, false) : addScript();
		} else {
			document.readyState == 'loading' ? document.attachEvent('onDOMContentLoaded', addScript, false) : addScript();
		}
	}

	function bl_track(co_account_id, group_id, country_id, version) {
		pc_track(co_account_id, group_id, country_id, version);
	}

	function get_target_param() {
		var target_params = microad_blade_gl.params;

		for (var i = 0; i < target_params.length; i++) {
			var co_account_id = target_params[i].co_account_id;
			var group_id = target_params[i].group_id;
			var country_id = target_params[i].country_id;
			var version = target_params[i].ver;

			var key = co_account_id + '_' + group_id;
			if(key in microad_blade_gl.complete_map) {
				continue;
			}

			bl_track(co_account_id, group_id, country_id, version);

			microad_blade_gl.complete_map[key] = target_params[i];
			break;
		}
	}

	get_target_param();

})();

