mergeInto(LibraryManager.library, {
    DownloadFile: function(filename, content) {
        var blob = new Blob([UTF8ToString(content)], { type: 'text/csv;charset=utf-8;' });
        var url = URL.createObjectURL(blob);

        var downloadLink = document.createElement("a");
        downloadLink.href = url;
        downloadLink.download = UTF8ToString(filename);

        document.body.appendChild(downloadLink);
        downloadLink.click();
        document.body.removeChild(downloadLink);
    }
});
