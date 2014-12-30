<?php
/**
* 3K Miles to a Cure: MOCK Service
* 
*/

require './mock-common.php'; 
require './mock-datasets.php'; 

$base = '/app-staff/api/1.0/';
$url = trim(str_replace($base, '', urldecode($_SERVER['REQUEST_URI'])));
$service_call = explode('/', $url);

header('Content-Type: application/json');

if (is_array($service_call))
{
	switch ($service_call[0])
	{
		case 'list':
			echo json_encode($staff_dataset);
			break;
		case 'read':
		case 'detail':
			//validate GUID
			if ($service_call[1] != null)
			{
				echo json_encode($staff_dataset[2]);
				exit;
			} else
			{
				error_message('missing staff guid');
			}
			break;
		case 'delete':
			if ($service_call[1] != null)
			{
				if ($_SERVER['REQUEST_METHOD'] == 'POST')
				{
					status_message('successfully deleted staff record.');
				} else
				{
					error_message(sprintf('%s request is not supported.', $_SERVER['REQUEST_METHOD']));
				}
			} else
			{
				error_message('missing staff guid');
			}
			break;
		case 'update':
			if ($service_call[1] == null)
			{
				error_message('missing staff guid');
				break;
			}
			//make sure it's POST request
			if ($_SERVER['REQUEST_METHOD'] == 'POST')
			{
				//make sure we have the fields
				if ($_POST['first_name'] && $_POST['last_name'] && $_POST['email'])
				{
					status_message('successfully updated staff record.');
				} else
				{
					error_message('Missing required fields');
				}
			} else
			{
				error_message(sprintf('%s request is not supported.', $_SERVER['REQUEST_METHOD']));
			}
			break;
		case 'create':
			//make sure it's POST request
			if ($_SERVER['REQUEST_METHOD'] == 'POST')
			{
				//make sure we have the fields
				if ($_POST['first_name'] && $_POST['last_name'] && $_POST['email'])
				{
					status_message('successfully created new staff record.');
				} else
				{
					error_message('Missing required fields');
				}
			} else
			{
				error_message(sprintf('%s request is not supported.', $_SERVER['REQUEST_METHOD']));
			}
			break;
		default:
			error_message('unsupported API call');
			break;
	}
} else
{
	error_message();
}
exit;

?>