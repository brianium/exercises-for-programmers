(ns brianium.madlib.input
  (:require [clojure.spec.alpha :as s]
            [clojure.string :as string]
            [brianium.madlib.spec :as madlib]))

(defn prompt
  [text]
  (print (str text " "))
  (flush)
  (-> (read-line)
      (string/trim)))

(defn prompt-text
  [[part-of-speech _]]
  (let [n (name part-of-speech)]
    (str "Enter " (if (contains? #{:adverb :adjective} part-of-speech) "an" "a") " " n ":")))

(s/fdef prompt-text
  :args (s/cat :part-of-speech ::madlib/ordered-part-of-speech)
  :ret  string?)

(defn ask
  [answers gen]
  (let [part-of-speech (gen (peek answers))]
    (->> (prompt-text part-of-speech)
         prompt
         (vector part-of-speech)
         (conj answers))))

(s/fdef ask
  :args (s/cat :answers ::madlib/answers :gen ::madlib/part-of-speech-gen)
  :ret  ::madlib/answers)
