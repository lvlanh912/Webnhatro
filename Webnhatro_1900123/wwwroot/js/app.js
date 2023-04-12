var quill = new Quill('#editor-container', {
    modules: {
        toolbar: [
            ['bold', 'italic'],
            ['link', 'blockquote', 'code-block'],
            [{ list: 'ordered' }, { list: 'bullet' }]
        ]
    },
    placeholder: 'Nhập nội dung của bạn vào đây',
    theme: 'snow'
});

var form = document.querySelector('form');
var delta = quill.clipboard.convert(document.getElementById('NoiDung').value);
quill.setContents(delta, 'silent');
document.getElementById('NoiDung').value = '';
function callMe() //display current HTML
{
    var html = quill.root.innerHTML;
    return html;
}
form.onsubmit = function () {
    // Populate hidden form on submit
    var NoiDung = document.querySelector('input[name=NoiDung]');
    NoiDung.value = callMe();
};