<?php
	/**
	* MOCK Admin View Processor: .NET Razor Template
	*/
	function get_angular_app($url)
	{
		$angular_apps = array(
							  'home'=>array('name'=>'adminApp', 'script'=>'/ui/app/admin/js/main'),
							  );
		
		switch ($url)
		{
			default:
				return $angular_apps['home'];
				
		}
	}

	$app = get_angular_app($_SERVER['REQUEST_URI']);
	$html  = file_get_contents($_SERVER['DOCUMENT_ROOT'] . '/Views/Admin/Index.cshtml');
	$html = str_replace('@Model.AppName', $app['name'], $html);
	$html = str_replace('@Model.AppScript', $app['script'], $html);
	$html = str_replace('@model 3KMilesCure.Admin.Models.MainAppViewModel', '', $html);
	echo $html;
	exit;
?>