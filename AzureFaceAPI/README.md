# Integrating Kendo UI with Azure Face API

The Kendo UI widgets can be seamlessly itegrated with a cognitive service such as [Azure Face API](https://azure.microsoft.com/en-us/services/cognitive-services/face/). The cloud-based Face API provides access to a set of advanced face algorithms, that enable developers to detect and identify faces in images.

## Send files directly to Face API with Kendo UI Upload

The below example demonstrates how files can be sent directly to the Azure Face API by using [the Kendo Upload's useArrayBuffer configuration option](https://docs.telerik.com/kendo-ui/api/javascript/ui/upload/configuration/async.usearraybuffer). In this way, the file is read with FileReader and the buffer data is then sent to the cloud service. In turn, the Face API analyzes the image, detects available faces and returns data in JSON format.

## Prerequisites

* [Create an Azure Account and subscribe to the FaceAPI service](https://azure.microsoft.com/en-us/services/cognitive-services/face/).

### Configuration

To successfully authenticate with the Face API service, peform the following simple steps:

* Copy the azure-faceapi-integration template.
* Replace the `subscriptionKey` value with your valid subscription key that is obtained from Azure.
* Change the `uriBase` value to use the location where you obtained your subscription keys.

## Breakdown of the implementation

1. The first step is to initialize an async Kendo UI Upload by also passing the base service URL with the required [Face API parameters](https://westcentralus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236) and also configuring the [`useArrayBuffer`](https://docs.telerik.com/kendo-ui/api/javascript/ui/upload/configuration/async.usearraybuffer) and [`withCredentials`](https://docs.telerik.com/kendo-ui/api/javascript/ui/upload/configuration/async.withcredentials) in order to send the uploaded file directly to the Azure Face API.

2. Next, to authenticate with the API, the `Content-Type` and `Ocp-Apim-Subscription-Key` headers have to be sent with the request. This is achieved through [the Upload's upload event](https://docs.telerik.com/kendo-ui/api/javascript/ui/upload/events/upload):
    ```js
    function onUpload(e) {
        var xhr = e.XMLHttpRequest;

        if (xhr) {
            xhr.addEventListener("readystatechange", function (e) {
                if (xhr.readyState == 1) {
                    xhr.setRequestHeader("Content-Type", "application/octet-stream");
                    xhr.setRequestHeader("Ocp-Apim-Subscription-Key", "subscriptionKey");
                }
            });
        }
    }
    ```

3. When the upload is successfully completed, the data from the Face API is received in JSON format. Then [the Upload's success event](https://docs.telerik.com/kendo-ui/api/javascript/ui/upload/events/success) is used to display the data and the image:
    ```js
    function onSuccess(e) {
        var upload = e.sender;
        var faceApiData = e.response;
        var fileData = e.files[0].rawFile;

        displayImage(fileData, "#imagePreview");
        $("#apiResponseData").val(JSON.stringify(faceApiData, null, 2));
    }

    function displayImage(file, containerId) {
        var fileReader = new FileReader();

        fileReader.onload = function (event) {
            var image = event.target.result;
            var containerElement = $(containerId);

            containerElement.attr('src', image);
        }
        fileReader.readAsDataURL(file);
    }
    ```

4. The received data from the Face API is based on the parameters that are initially passed to the service and returns recognized faces coordinates, gender, age, emotion and other face attirubutes:
    ```json
    [
      {
        "faceId": "937f09d1-6476-4d14-b92b-c20641e84268",
        "faceRectangle": {
          "top": 68,
          "left": 63,
          "width": 101,
          "height": 101
        },
        "faceAttributes": {
          "smile": 0.008,
          "headPose": {
            "pitch": 0,
            "roll": -0.2,
            "yaw": -0.2
          }
        },
        ...
      }
    ]
    ```