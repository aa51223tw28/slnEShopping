$(document).ready(function () {
    $('#reportModal').on('show.bs.modal', function (event) {
        //獲取觸發窗口的按鈕
        var button = $(event.relatedTarget);

        // 從 data- 屬性中提取出 userId 和 productId
        var userId = button.data('userid');
        var productId = button.data('productid');

        // 更新窗口的內容。
        var modal = $(this);
        modal.find('#UserId').val(userId);
        modal.find('#ProductId').val(productId);
    });
});
function submitComplaint() {
    var formData = new FormData($('#complaintForm')[0]);

    $.ajax({
        url: '/Support2/CSSendMail',
        type: 'POST',
        data: formData,
        processData: false,  // 告訴jQuery不要處理傳送的數據
        contentType: false,  // 告訴jQuery不要設置contentType
        success: function (data) {
            alert('投訴單已寄出');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert('寄送失敗');
        },
        complete: function () {
            $('#reportModal').modal('hide');
        }
    });
}

//看圖片用
$(document).ready(function () {
    $('#ImageFile').change(function (e) {
        var reader = new FileReader();

        reader.onload = function (e) {
            // 顯示圖片預覽
            $('#imagePreview').attr('src', e.target.result);
            $('#imagePreview').removeAttr('hidden');
        }

        reader.readAsDataURL(this.files[0]);
    });
});